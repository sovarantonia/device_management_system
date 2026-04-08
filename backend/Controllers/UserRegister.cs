using backend.Entity.DTO;
using backend.Service;

namespace backend.Controllers
{
    public static class UserRegister
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("register", async(UserRequest request,
                IUserService userService
                ) =>
            {
                var user = await userService.RegisterAsync(request);
                return Results.Ok(user);
            }
                );
        }
    }
}
