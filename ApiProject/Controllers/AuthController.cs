using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Dtos.UserDtos;
using Service.Services.Interfaces;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(IAuthService authService, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _authService = authService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet("users")]
        public async Task<IActionResult> CreateUser()
        {
            /*if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await _roleManager.RoleExistsAsync("Member"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Member"));
            }
*/
            AppUser user1 = new AppUser
            {
                FullName = "Member",
                UserName = "member",
            };
            await _userManager.CreateAsync(user1, "Member123");


            AppUser user2 = new AppUser
            {
                FullName = "Admin",
                UserName = "admin",
            };
            await _userManager.CreateAsync(user2, "Admin123");
            
            try
            {
                var result1 = await _userManager.AddToRoleAsync(user1, "Member");

            }
            catch (Exception e)
            {

                throw e;
            }
            var result2 = await _userManager.AddToRoleAsync(user2, "Admin");

            return Ok(user1.Id);
        }

        [HttpPost("login")]
        public ActionResult Login(UserLoginDto loginDto)
        {
            var token = "Bearer " + _authService.Login(loginDto);
            return Ok(new { token });
        }


        [Authorize]
        [HttpGet("profile")]
        public ActionResult Profile()
        {
            return Ok(User.Identity.Name);
        }
    }
}
