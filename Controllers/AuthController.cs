using Microsoft.AspNetCore.Identity;
using NZWalksAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Repositories;
namespace NZWalksAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly ITokenRepository tokenRepository;

    // Register an User by Injecting the UserManager class
    public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
    {
        this.userManager = userManager;
        this.tokenRepository = tokenRepository;
    }

    // POST: /api/Auth/Register
    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var identityUser = new IdentityUser
        {
            UserName = registerRequestDto.Username,
            Email = registerRequestDto.Username
        };
        var identityresult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);
        if (identityresult.Succeeded)
        {
            // Add roles to the user
            if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
            {
                identityresult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                if (identityresult.Succeeded)
                {
                    return Ok("User was registered! Please login");
                }
            }
        }
        return BadRequest("Something went wrong");
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {

        var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
        if (user != null)
        {
            var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (checkPasswordResult)
            {
                // Get roles for the user
                var roles = await userManager.GetRolesAsync(user);
                if (roles != null)
                {
                    // Create a JWT Token
                    var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                    var response = new LoginResponseDto
                    {
                        JwtToken = jwtToken
                    };
                    return Ok(response);
                }
            }
        }
        return BadRequest("Username or Password is incorrect");
    }

}