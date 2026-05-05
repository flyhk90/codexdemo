using Microsoft.AspNetCore.Mvc;
using backend_csharp.Models;

namespace backend_csharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public ActionResult<HealthResponse> Get()
    {
        return Ok(new HealthResponse
        {
            Status = "Healthy",
            Message = "后端接口运行正常，前后端已可联调。",
            ServerTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        });
    }
}
