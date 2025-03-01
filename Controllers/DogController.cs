using DogsSanctuary.Authenticator;
using DogsSanctuary.Db;
using Microsoft.AspNetCore.Mvc;

namespace DogsSanctuary.Controllers;

[ApiController]
[Route("[controller]")]
public class DogController : ControllerBase
{
    private readonly ITokenManager _tokenManager;

    public DogController(ITokenManager tokenManager)
    {
        this._tokenManager = tokenManager;
    }
    
    [HttpPost("GenToken")]
    public IActionResult GenToken([FromBody] GenTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.BestFriend))
        {
            return BadRequest("BestFriend is required");
        }

        var dog = RepoDogs.Dogs.FirstOrDefault(x => x.BestFriend == request.BestFriend);

        if (dog is null)
            return BadRequest("BestFriend not found");

        var token = _tokenManager.GenerateToken(dog);

        return Ok(new GenTokenResponse(token));
    }

    public record GenTokenRequest(string BestFriend);

    public record GenTokenResponse(string token);
}