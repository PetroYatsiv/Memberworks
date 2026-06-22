namespace MemberWorks.Infrastructure.Authentication;

public class GoogleAuthOptions
{
    public const string SectionName = "Google";
    
    // Oauth 2 ClientId
    public string ClientId { get; set; } = string.Empty;
}