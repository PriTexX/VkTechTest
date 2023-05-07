using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using VkTechTest.Database;
using VkTechTest.Models.Enums;
using VkTechTest.Services.Interfaces;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IUserService _userService;

    public BasicAuthenticationHandler(IOptionsMonitor<RemoteAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IUserService userService, ApplicationContext applicationContext) : base(options, logger, encoder, clock)
    {
        _userService = userService;
    }
    
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        if (!authorizationHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
        {
            Response.StatusCode = 401;
            Response.Headers.Add("WWW-Authenticate", "Basic");
            return AuthenticateResult.Fail("Invalid Authorization Header");    
        }
        
        var token = authorizationHeader.Substring("Basic ".Length).Trim();
        var credentialsAsEncodedString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
        var credentials = credentialsAsEncodedString.Split(':');

        var user = await _userService.AuthenticateAsync(credentials[0], credentials[1]);

        if (user is null)
        {
            Response.StatusCode = 401;
            Response.Headers.Add("WWW-Authenticate", "Basic");
            return AuthenticateResult.Fail("Invalid username or password");
        }
        
        var claims = new List<Claim>
        {
            new Claim("username", credentials[0]), 
            new Claim(ClaimTypes.Role, "User"),
        };

        if (user.UserGroup.Code == UserGroupType.Admin)
        {
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        }
        
        var identity = new ClaimsIdentity(claims, "Basic");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
    }
}