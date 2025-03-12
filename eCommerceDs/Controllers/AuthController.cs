using eCommerceDs.DTOs;
using eCommerceDs.Models;
using eCommerceDs.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerceDs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly eCommerceDsContext _context;
        private readonly ITokenService _tokenService;
        private readonly HashService _hashService;

        public AuthController(eCommerceDsContext context, HashService hashService, ITokenService tokenService)
        {
            _context = context;
            _hashService = hashService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserLoginDTO user)
        {
            var userDB = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (userDB == null)
            {
                return Unauthorized("User not found");
            }

            var hashResult = _hashService.Hash(user.Password, userDB.Salt);
            if (userDB.Password != hashResult.Hash)
            {
                return Unauthorized("Invalid credentials");
            }

            var tokenResponse = _tokenService.GenerateToken(user);
            return Ok(tokenResponse);
        }
    }
}

