using API.DTOs;
using Domain;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _singInManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> singInManager, TokenService tokenService)
        {
            this._userManager = userManager;
            this._singInManager = singInManager;
            this._tokenService = tokenService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.Users
                .Include(user => user.Photos)
                .FirstOrDefaultAsync(user => user.Email == loginDTO.Email);

            if (user == null) return Unauthorized();

            var result = await _singInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (result.Succeeded)
            {
                return Ok(CreateUserDTO(user));
            }

            return Unauthorized();

        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await _userManager.Users.AnyAsync(user => user.Email == registerDTO.Email))
            {
                ModelState.AddModelError("email", "Email taken");
                return ValidationProblem(ModelState);
            }
            if (await _userManager.Users.AnyAsync(user => user.UserName == registerDTO.UserName))
            {
                ModelState.AddModelError("username", "Username taken");
                return ValidationProblem(ModelState);
            }

            var user = new AppUser
            {
                DisplayName = registerDTO.DisplayName,
                Email = registerDTO.Email,
                UserName = registerDTO.UserName
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded) return Ok(CreateUserDTO(user));

            return BadRequest("Problem registering user!");
        }

        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await _userManager.Users
                .Include(user => user.Photos)
                .FirstOrDefaultAsync(user => user.Email == User.FindFirstValue(ClaimTypes.Email));

            return Ok(CreateUserDTO(user));
        }

        private UserDTO CreateUserDTO(AppUser user)
        {
            var userDTO = new UserDTO()
            {
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user),
                UserName = user.UserName,
                Image = user?.Photos?.FirstOrDefault(photo => photo.IsMain)?.Url
            };

            return userDTO;
        }

    }
}
