using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/access")]
    public class AccessController : Controller
    {
        [HttpGet("client")]
        [Authorize(Roles = "Client")]
        public IActionResult ClientAccess()
        {
            return Ok(new
            {
                Message = "Client access granted."
            });
        }

        [HttpGet("driver")]
        [Authorize(Roles = "Driver")]
        public IActionResult DriverAccess()
        {
            return Ok(new
            {
                Message = "Driver access granted."
            });
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminAccess()
        {
            return Ok(new
            {
                Message = "Admin access granted."
            });
        }
    }
}
