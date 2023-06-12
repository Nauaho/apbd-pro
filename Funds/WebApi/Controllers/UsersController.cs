using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApi.Models.DTOs;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IConfiguration _configuration;

        public UsersController(IUsersRepository usersRepository, IConfiguration configuration)
        {
            _usersRepository = usersRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(RegisterOrLoginRequest rl)
        {
            if (await _usersRepository.CheckUserAsync(rl))
            {

                return Ok(new { Token = GenerateJWT() });
            }
            else { return Unauthorized(); }

        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterOrLoginRequest rl)
        {

            var res = await _usersRepository.AddUserAsync(rl);
            if (res == null)
                return BadRequest();
            return Created($"api/user/{res.Login}", new { res.RefreshToken, Token = GenerateJWT() });
        }

        [HttpPost("refresh/token")]
        public async Task<IActionResult> NewAccessToken(RefreshToken rt)
        {
            if (await _usersRepository.UpdateRefreshtokenAsync(rt.RToken))
                return Ok(GenerateJWT());
            else { return Unauthorized(); }
        }

        private string? GenerateJWT()
        {
            var key = _configuration["JWT:SecretKey"] ?? throw new NullReferenceException();
            var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
            (
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                signingCredentials: creds,
                expires: DateTime.Now.AddMinutes(5)
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
