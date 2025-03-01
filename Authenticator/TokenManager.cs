using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace DogsSanctuary.Authenticator;

internal sealed class TokenManager() : ITokenManager
{
    private readonly IConfiguration _configuration;

    public TokenManager(IConfiguration configuration) : this()
    {
        this._configuration = configuration;
    }

    public string GenerateToken(Dog dog)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? string.Empty));

        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Sub, dog.DogName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var bestFriend in dog.BestFriend)
        {
            claims.Add(new Claim(ClaimTypes.Role, bestFriend.ToString()));
        }

        var expirationTimeInMinutes = jwtSettings.GetValue<int>("ExpirationTimeInMinutes");
        
        var token = new JwtSecurityToken(
            issuer: jwtSettings.GetValue<string>("Issuer"),
            audience: jwtSettings.GetValue<string>("Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationTimeInMinutes),
            signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256));
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}