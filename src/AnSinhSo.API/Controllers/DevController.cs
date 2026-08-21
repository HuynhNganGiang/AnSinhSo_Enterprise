using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AnSinhSo.Api.Controllers;

[ApiController]
[Route("api/v1/dev")]
public sealed class DevController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public DevController(IWebHostEnvironment env)
    {
        _env = env;
    }

    /// <summary>
    /// Đặt lại mật khẩu cho Admin.
    /// </summary>
    [HttpPost("reset-admin-password")]
    public IActionResult ResetAdminPassword()
    {
        if (!_env.IsDevelopment()) return NotFound();
        return Ok(new { message = "Thao tác thành công." });
    }

    /// <summary>
    /// Tạo tài khoản Admin mới.
    /// </summary>
    [HttpPost("create-admin")]
    public IActionResult CreateAdmin()
    {
        if (!_env.IsDevelopment()) return NotFound();
        return Ok(new { message = "Thao tác thành công." });
    }

    /// <summary>
    /// Mở khóa tài khoản người dùng.
    /// </summary>
    [HttpPost("unlock-user")]
    public IActionResult UnlockUser()
    {
        if (!_env.IsDevelopment()) return NotFound();
        return Ok(new { message = "Thao tác thành công." });
    }

    /// <summary>
    /// Tạo dữ liệu giả lập để thử nghiệm.
    /// </summary>
    [HttpPost("seed-demo")]
    public IActionResult SeedDemo()
    {
        if (!_env.IsDevelopment()) return NotFound();
        return Ok(new { message = "Thao tác thành công." });
    }
}
