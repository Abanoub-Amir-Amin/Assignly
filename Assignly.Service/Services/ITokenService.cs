using System.Security.Claims;

namespace Assignly.Service.Services;

public interface ITokenService
{
    public string GenerateToken(Dictionary<string, string> claims);
    public ClaimsPrincipal ValidateToken(string token);
}
