using eCommerceDs.DTOs;
using eCommerceDs.Models;
using eCommerceDs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eCommerceDs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly eCommerceDsContext _context;
        private readonly HashService _hashService;
        private readonly IConfiguration _configuration;

        public UsersController(eCommerceDsContext context,
        HashService hashService,
        IConfiguration configuration)
        {
            _context = context;
            _hashService = hashService;
            _configuration = configuration;
        }


        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] UserInsertDTO userInsertDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var resultHash = _hashService.Hash(userInsertDTO.Password);
            var role = string.IsNullOrWhiteSpace(userInsertDTO.Role) || userInsertDTO.Role.Trim().ToLower() == "string"
            ? "User"
            : userInsertDTO.Role.Trim();

            var allowedRoles = new List<string> { "User", "Admin" };
            if (!allowedRoles.Contains(role))
            {
                return BadRequest("Invalid role");
            }

            var newUser = new User
            {
                Email = userInsertDTO.Email,
                Password = resultHash.Hash,
                Salt = resultHash.Salt,
                Role = role
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return Ok(newUser);
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] UserLoginDTO user)
        {
            var userDB = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (userDB == null)
            {
                return BadRequest("User not found");
            }

            var resultHash = _hashService.Hash(user.Password, userDB.Salt);
            if (userDB.Password == resultHash.Hash)
            {
                var response = GenerateToken(user);
                return Ok(response);
            }
            else
            {
                return Unauthorized("Invalid password");
            }
        }

        private LoginResponseDTO GenerateToken(UserLoginDTO credentialsUser)
        {
            var userDB = _context.Users.FirstOrDefault(x => x.Email == credentialsUser.Email);
            if (userDB == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, credentialsUser.Email),
                new Claim(ClaimTypes.Role, userDB.Role), 
                new Claim("whatever i want", "any other value")  
            };

            var key = _configuration["JWTKey"];
            var keyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signinCredentials = new SigningCredentials(keyKey, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new LoginResponseDTO()
            {
                Token = tokenString,
                Email = credentialsUser.Email
            };
        }


        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] UserChangePasswordDTO userChangePasswordDTO)
        {
            var userDB = await _context.Users.FirstOrDefaultAsync(x => x.Email == userChangePasswordDTO.Email);

            if (userDB == null)
            {
                return Unauthorized("User not found");
            }

            var resultHash = _hashService.Hash(userChangePasswordDTO.Password, userDB.Salt);

            if (userDB.Password != resultHash.Hash)
            {
                return Unauthorized("The current password is incorrect");
            }

            var newResultHash = _hashService.Hash(userChangePasswordDTO.NewPassword);

            userDB.Password = newResultHash.Hash;
            userDB.Salt = newResultHash.Salt;

            _context.Users.Update(userDB);
            await _context.SaveChangesAsync();

            return Ok("Password updated successfully");
        }


        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }


        [HttpDelete("{email}")]
        public async Task<ActionResult> Delete(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user is null)
            {
                return NotFound();
            }

            _context.Remove(user);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
