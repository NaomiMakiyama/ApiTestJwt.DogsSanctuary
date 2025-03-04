using DogsSanctuary.Db;

namespace DogsSanctuary.Authenticator;

public interface ITokenManager
{
    string GenerateToken(Dog dog);
    string GenerateRefreshToken(Dog dog);
    
    Task<(bool isValid, string? dogName)> ValidateTokenAsync(string token); 
}