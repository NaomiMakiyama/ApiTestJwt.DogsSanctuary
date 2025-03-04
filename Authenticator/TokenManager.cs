using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DogsSanctuary.Db;
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

        foreach (var characteristic in dog.Characteristics)
        {
            claims.Add(new Claim(ClaimTypes.Role, characteristic.Description));
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

    public string GenerateRefreshToken(Dog dog)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(jwtSettings["SecretKey"] ?? string.Empty));
        
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, dog.DogName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        
        var expirationTimeInMinutes = jwtSettings.GetValue<int>("RefreshExpirationTimeInMinutes");
        
        var token = new JwtSecurityToken(
            issuer: jwtSettings.GetValue<string>("Issuer"), 
            audience: jwtSettings.GetValue<string>("Audience"), 
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationTimeInMinutes),
            signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256));
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public async Task<(bool isValid, string? dogName)> ValidateTokenAsync(string token)
    {
        if(string.IsNullOrWhiteSpace(token))
            return (false, null);
        
        var tokenParameters = TokenHelpers.GetTokenValidationParameters(_configuration);
        var validTokenResult = await new JwtSecurityTokenHandler().ValidateTokenAsync(token, tokenParameters);
        
        if (!validTokenResult.IsValid)
            return (false, null);
        
        var userName = validTokenResult
            .Claims.FirstOrDefault(c => c.Key == ClaimTypes.NameIdentifier).Value as string;

        return (true, userName);
    }
}