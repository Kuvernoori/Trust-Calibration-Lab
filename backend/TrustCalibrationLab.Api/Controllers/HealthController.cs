using Microsoft.AspNetCore.Mvc;

namespace TrustCalibrationLab.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public ActionResult<object> Get() => Ok(new
    {
        status = "ok",
        service = "trust-calibration-lab-api"
    });
}
