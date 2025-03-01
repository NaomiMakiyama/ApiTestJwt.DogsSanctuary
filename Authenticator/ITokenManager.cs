namespace DogsSanctuary.Authenticator;

public interface ITokenManager
{
    string GenerateToken(Dog dog);
}