using MediatR;
using MemberWorks.Application.Authentication.Dtos;

namespace MemberWorks.Application.Authentication.Commands.GoogleLogin;

public record GoogleLoginCommand(string IdToken) : IRequest<AuthResponse>;