using Microsoft.AspNetCore.Mvc;

namespace Chess.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(ILogger<AuthorizationController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("~/authorization/register")]
        public string Register()
        {
            return "register";
        }

        [HttpPost]
        [Route("~/account/login")]
        public void Login()
        {

        }
    }
}
