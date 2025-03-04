using DogsSanctuary.Authenticator;
using DogsSanctuary.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogsSanctuary.Controllers;

[ApiController]
[Route("[controller]")]
public class DogController(ITokenManager tokenManager) : ControllerBase
{
    [HttpPost("gen-token")]
    public IActionResult GenToken([FromBody] GenTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.DogName))
        {
            return BadRequest("BestFriend is required");
        }

        var dog = RepoDogs.Dogs.FirstOrDefault(x => x.DogName == request.DogName);

        if (dog is null)
            return BadRequest("BestFriend not found");

        var token = tokenManager.GenerateToken(dog);
        var refreshToken = tokenManager.GenerateRefreshToken(dog);

        return Ok(new GenTokenResponse(token, refreshToken));
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return BadRequest();

        var isValidTokenResult = await tokenManager.ValidateTokenAsync(request.RefreshToken);

        if (!isValidTokenResult.isValid)
            return Unauthorized();

        var dogName = isValidTokenResult.dogName;
        var dog = RepoDogs.Dogs.FirstOrDefault(x => x.DogName == dogName);

        if (dog is null)
            return Unauthorized();

        var token = tokenManager.GenerateToken(dog);
        var refreshToken = tokenManager.GenerateRefreshToken(dog);

        return Ok(new GenTokenResponse(token, refreshToken));
    }
    [Authorize]
    [HttpGet("auth")]
    public string auth() => "Is authenticated";

[Authorize]
    [HttpGet("get-dog")]
    public IActionResult GetDog(string dogName)
    {
        var dog = RepoDogs.Dogs.FirstOrDefault(x => x.DogName == dogName);
        
        if (dog is null)
            return BadRequest("Dog not found");
        
        return Ok(dog);
    }
    
    [Authorize(Roles = RepoDogs.IS_TRAINED)]
    [HttpGet("is-trained")]
    public string IsTrained() => "This dog is trained";
    
    [Authorize(Roles = RepoDogs.IS_FRIENDLY)]
    [HttpGet("is-friendly")]
    public string IsFriendly() => "This dog is friendly";
    
    [Authorize(Roles = RepoDogs.CAN_SWIN)]
    [HttpGet("can-swin")]
    public string CanSwin() => "This dog can swin";
    
    public record GenTokenRequest(string DogName);

    public record RefreshTokenRequest(string RefreshToken);

    private record GenTokenResponse(string Token, string RefreshToken);
   
}