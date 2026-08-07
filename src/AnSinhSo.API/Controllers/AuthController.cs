using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AnSinhSo.Application.Authentication.Login;
using AnSinhSo.Contracts.Authentication;

namespace AnSinhSo.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.UsernameOrEmail, request.Password);
        
        var result = await _sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            if (result.Error.Code == "Auth.InvalidCredentials")
            {
                return Unauthorized(result);
            }
            
            return BadRequest(result);
        }
        
        return Ok(result);
    }
}

