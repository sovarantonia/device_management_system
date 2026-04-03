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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await userService.GetByIdAsync(id);

            return Ok(UserMapper.ToDTO(user));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        { 
            var users = await userService.GetAllAsync();
            return Ok(users.Select(u => UserMapper.ToDTO(u)));
        }

        [HttpPost]
        public async Task<IActionResult> SaveUser([FromBody] UserRequest userRequest)
        {
            var user = await userService.SaveAsync(userRequest);

            return Ok(UserMapper.ToDTO(user));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        { 
            await userService.DeleteAsync(id); 

            return Ok(); 
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserRequest request)
        {
            var user = await userService.UpdateAsync(id, request);

            return Ok(UserMapper.ToDTO(user));
        }
    }
}
