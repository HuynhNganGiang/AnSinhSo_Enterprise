using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AnSinhSo.API.Controllers;

namespace AnSinhSo.Api.Controllers;

public sealed record RegisterCitizenRequest(string Phone, string FullName, string IdentityNumber);

[ApiController]
[Route("api/v1/citizens")]
public sealed class CitizensController : ApiControllerBase
{
    /// <summary>
    /// Đăng ký tài khoản công dân mới.
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public IActionResult RegisterCitizen([FromBody] RegisterCitizenRequest request)
    {
        return Ok(new { success = true, message = "Đã gửi yêu cầu đăng ký công dân." });
    }
}
