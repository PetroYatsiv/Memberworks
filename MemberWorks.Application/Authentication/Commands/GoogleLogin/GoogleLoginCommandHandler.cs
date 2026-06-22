using MediatR;
using MemberWorks.Application.Authentication.Dtos;
using MemberWorks.Application.Common.Exceptions;
using MemberWorks.Application.Common.Interfaces;
using MemberWorks.Domain.Entities;
using MemberWorks.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MemberWorks.Application.Authentication.Commands.GoogleLogin;

public class GoogleLoginCommandHandler(
    IApplicationDbContext db,
    IGoogleTokenValidator googleValidator,
    IJwtTokenService jwt)
    : IRequestHandler<GoogleLoginCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(GoogleLoginCommand request, CancellationToken ct)
    {
        var profile = await googleValidator.ValidateAsync(request.IdToken, ct)
            ?? throw new ForbiddenAccessException("The Google token could not be verified.");

        // No caller is authenticated yet, so the tenant query filter must be bypassed here.
        var user = await db.Users
            .IgnoreQueryFilters()
            .Include(u => u.Organization)
            .FirstOrDefaultAsync(u => u.GoogleSubjectId == profile.Subject, ct);

        if (user is null)
        {
            // Link an existing invited user by email, or provision a brand-new organization.
            user = await db.Users
                .IgnoreQueryFilters()
                .Include(u => u.Organization)
                .FirstOrDefaultAsync(u => u.Email == profile.Email, ct);

            if (user is not null)
            {
                user.GoogleSubjectId = profile.Subject;
            }
            else
            {
                user = ProvisionNewOrganization(profile);
                db.Organizations.Add(user.Organization);
                db.Users.Add(user);
            }

            await db.SaveChangesAsync(ct);
        }

        var token = jwt.CreateToken(user);
        return new AuthResponse(token, CurrentUserDto.FromEntity(user, user.Organization.Name));
    }

    private static User ProvisionNewOrganization(Common.Models.GoogleUserInfo profile)
    {
        var organization = new Organization
        {
            Name = $"{(profile.GivenName ?? profile.Email)}'s Organization"
        };

        return new User
        {
            Organization = organization,
            OrganizationId = organization.Id,
            Email = profile.Email,
            FirstName = profile.GivenName ?? string.Empty,
            LastName = profile.FamilyName ?? string.Empty,
            GoogleSubjectId = profile.Subject,
            Role = ApplicationRole.OrgAdmin // first user of a new org is its admin
        };
    }
}