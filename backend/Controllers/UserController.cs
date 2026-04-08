using backend.Entity.DTO;
using backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await userService.GetByIdAsync(id);

            return Ok(UserMapper.ToDTO(user));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = userService.GetAll();
            return Ok(users.Select(u => UserMapper.ToDTO(u)));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await userService.DeleteAsync(id);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserRequest request)
        {
            var user = await userService.UpdateAsync(id, request);

            return Ok(UserMapper.ToDTO(user));
        }
    }
}
