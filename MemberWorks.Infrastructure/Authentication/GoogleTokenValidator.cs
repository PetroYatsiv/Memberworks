using Google.Apis.Auth;
using MemberWorks.Application.Common.Interfaces;
using MemberWorks.Application.Common.Models;
using Microsoft.Extensions.Options;

namespace MemberWorks.Infrastructure.Authentication;

public class GoogleTokenValidator(IOptions<GoogleAuthOptions> options) : IGoogleTokenValidator
{
    private readonly GoogleAuthOptions _options = options.Value;

    public async Task<GoogleUserInfo?> ValidateAsync(string idToken, CancellationToken cancellationToken = default)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = [_options.ClientId]
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            return new GoogleUserInfo(payload.Subject, payload.Email, payload.GivenName, payload.FamilyName);
        }
        catch (InvalidJwtException)
        {
            return null;
        }
    }
}