namespace MemberWorks.Application.Authentication.Dtos;

public record AuthResponse(string Token, CurrentUserDto User);
