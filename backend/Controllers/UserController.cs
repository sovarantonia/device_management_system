using backend.Entity;
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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await userService.GetByIdAsync(id);
            
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        { 
            var users = await userService.GetAllAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> SaveUser([FromBody] User user)
        {
            var created = await userService.SaveAsync(user);
            if (!created)
            {
                return Conflict("Email already exists");
            }

            return Ok(user);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        { 
            var deleted = await userService.DeleteAsync(id); 
            if (!deleted)
            {
                return NotFound("User not found");
            }

            return Ok(); 
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserUpdateRequest request)
        {
            var updated = await userService.UpdateAsync(id, request);
            if (!updated)
            {
                return NotFound("User not found");
            }
            var user = await userService.GetByIdAsync(id);

            return Ok(user);
        }
    }
}
