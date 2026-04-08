using backend.Entity;
using backend.Entity.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace backend.Controllers
{
    public static class LoginUser
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("login", async (
                UserLogin userLoginRequest,
                UserManager<User> userManager,
                IConfiguration configuration
                ) =>
            {
                var user = await userManager.FindByEmailAsync(userLoginRequest.Email);
                if (user == null || !await userManager.CheckPasswordAsync(user, userLoginRequest.Password))
                {
                    return Results.Unauthorized();
                }

                var signingKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));
                var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

                List<Claim> claims =
                [
                    new (JwtRegisteredClaimNames.Sub, user.Id),
                    new (JwtRegisteredClaimNames.Email, user.Email),
                ];

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
                    SigningCredentials = credentials,
                    Issuer = configuration["Jwt:Issuer"],
                    Audience = configuration["Jwt:Audience"]
                };

                var tokenHandler = new JsonWebTokenHandler();

                string accessToken = tokenHandler.CreateToken(tokenDescriptor);

                return Results.Ok(new UserLoginResponse { User = UserMapper.ToDTO(user), Token = accessToken });
            });
        }
    }
}
