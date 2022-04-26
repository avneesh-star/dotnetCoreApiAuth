using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Registeruser(RegistrationModel model)
        {
            var response = await userService.RegisterUser(model);
            return Ok(response);
        }


        [HttpPost("login")]

        public async Task<IActionResult> UserLogin(LoginModel model)
        {
            var response = await userService.Login(model);
            return Ok(response);
        }

    }
}
