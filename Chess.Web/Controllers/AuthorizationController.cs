using Chess.Web.Data;
using Chess.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Chess.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly ILogger<AuthorizationController> _logger;
        private readonly ChessContext context;

        public AuthorizationController(ILogger<AuthorizationController> logger)
        {
            _logger = logger;
            context = new ChessContext();
        }

        [HttpPost]
        [Route("~/authorization/register")]
        public string Register([FromBody] object data)
        {
            try
            {
                var registration = JsonConvert.DeserializeObject<Registration>(data.ToString());

                if (registration is null)
                    return "failed";

                if (context.Users.Any(x => x.NormalizedEmail == registration.Email.ToLower()))
                    return "failed";

                if (string.IsNullOrWhiteSpace(registration.Password))
                    return "failed";

                if (!new EmailAddressAttribute().IsValid(registration.Email))
                    return "failed";

                if (context.Users.Any(x => x.NormalizedEmail == registration.Email.ToLower()))
                    return "failed";

                var salt = Encryption.Encryption.GenerateSalt();
                var hash = Encryption.Encryption.GenerateHash(registration.Password, salt);
                var passwordHash = $"{hash}{salt}";

                var newUser = new User()
                {
                    Email = registration.Email,
                    NormalizedEmail = registration.Email.ToLower(),
                    UserName = registration.Username,
                    NormalizedUserName = registration.Username.ToLower(),
                    PasswordHash = passwordHash
                };

                context.Users.Add(newUser);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                return "failed";
            }

            return "success";
        }

        [HttpPost]
        [Route("~/account/login")]
        public string Login([FromBody] object data)
        {
            try
            {
                var login = JsonConvert.DeserializeObject<Login>(data.ToString());

                if (login is null)
                    return "failed";

                var signInManager = new SignInManager();
            }
            catch
            {

            }
        }
    }
}
