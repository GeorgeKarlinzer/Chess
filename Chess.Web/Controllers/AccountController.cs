using Chess.Web.Data;
using Chess.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Chess.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ChessContext context;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
            context = new ChessContext();
        }

        [HttpGet]
        [Route("~/account/issignedin")]
        public bool IsSignedIn()
        {
            return User?.Identity?.IsAuthenticated == true;
        }

        [HttpPost]
        [Route("~/account/logout")]
        public void ChessSignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
        }

        [HttpPost]
        [Route("~/account/register")]
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
            catch (Exception ex)
            {
                return "failed";
            }

            return "success";
        }

        [HttpPost]
        [Route("~/account/login")]
        public async Task<string> Login([FromBody] Login data)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == data.Username);

                if (user is null)
                    return "Incorrect username or password";

                var (salt, passwordHash) = (user.PasswordHash[32..], user.PasswordHash[..32]);
                var enteredPassHash = Encryption.Encryption.GenerateHash(data.Password, salt);

                if (passwordHash != enteredPassHash)
                    return "Incorrect username or password";

                var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.Id)),
                        new Claim(ClaimTypes.Name, user.UserName)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return "success";
            }
            catch
            {
                return "failed";
            }
        }
    }
}
