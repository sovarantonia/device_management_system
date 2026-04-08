using Azure.Core;
using backend.Entity;
using backend.Entity.DTO;
using backend.Helper;
using backend.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private readonly IUserService userService;

        public AuthController(UserManager<User> userManager, IConfiguration configuration, IUserService userService)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest userLoginRequest)
        {
            var user = await userManager.FindByEmailAsync(userLoginRequest.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, userLoginRequest.Password))
            {
                return Unauthorized("Wrong credentials");
            }

            var accessToken = JwtHelper.GenerateJwtToken(configuration, user);

            return Ok(new UserLoginResponse { User = UserMapper.ToDTO(user), Token = accessToken });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRequest userRequest)
        {
            var user = await userService.RegisterAsync(userRequest);
            return Ok(user);
        }
    }
}
