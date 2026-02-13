using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities.IdentityEntities;
using Store.Service.HandelResponses;
using Store.Service.Services.UserService;
using Store.Service.Services.UserService.Dtos;


namespace Store.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(IUserService userService, UserManager<AppUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto input)
        {
            var user = await _userService.Login(input);
            if (user is null)
                return BadRequest(new CustomException(400, "Email or Password is incorrect"));

            return Ok(user);
        }
        [HttpPost]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto input)
        {
            var user = await _userService.Register(input);
            if (user is null)
                return BadRequest(new CustomException(400, "Email is already taken"));

            return Ok(user);
        }
        [HttpGet]
        [Authorize]
        public async Task<UserDto> GetCurrentUserDetails()
        {
            var userId = User?.FindFirstValue("userId");
            var user = await _userManager.FindByIdAsync(userId);

            return new UserDto
            {
                Id = Guid.Parse(user.Id),
                DisplayName = user.DisplayName,
                Email = user.Email
            };
        }
    }
}