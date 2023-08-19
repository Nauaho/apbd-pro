using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
            var rt = await _usersRepository.CheckUserAsync(rl);
            if (rt is null)
                return Unauthorized();
            return Ok(new {RefreshToken = rt.Token, AccessToken = GenerateJWT(rt.Session) });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterOrLoginRequest rl)
        {

            var res = await _usersRepository.AddUserAsync(rl);
            if (res == null)
                return BadRequest();
            return Created($"api/users/{res.UserLogin}", new { RefreshToken = res.Token, AccessToken = GenerateJWT(res.Session) });
        }

        [HttpPost("refresh/token")]
        public async Task<IActionResult> NewAccessToken(RefreshToken rt)
        {
            var res = await _usersRepository.UpdateRefreshTokenAsync(rt.RToken);
            if (res is not null)
                return Ok(new { RefreshToken = res.Token, AccessToken = GenerateJWT(res.Session) });
            else { return Unauthorized(); }
        }

        private string? GenerateJWT(string session)
        {
            var claimList = new List<Claim>
            {
                new Claim("session", session)
            }; 
            var key = _configuration["JWT:SecretKey"] ?? throw new NullReferenceException();
            var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
            (
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                signingCredentials: creds,
                expires: DateTime.Now.AddMinutes(5),
                claims: claimList
            ) ;
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout(string token)
        {
            var aTSession = Request.Headers.Authorization[0]?.Split(' ')[1];
            if (token == null)
                return BadRequest();
            var result = await _usersRepository.LogUserOut(token, aTSession);
            if (result)
                return Ok();

            return NotFound();
        }

        [Authorize]
        [HttpGet("{login}/subs")]
        public async Task<IActionResult> GetSubs(string login)
        {
            var r = await _usersRepository.GetSubscriptionAsync(login);
            if (r == null) 
                return NotFound();
            var result = r.Select(t => new TickerDTO
            {
                Name = t.Name,
                Market = t.Market,
                Locale = t.Locale,
                PrimaryExchange = t.PrimaryExchange,
                Type = t.Type,
                Active = t.Active,
                CurrencyName = t.CurrencyName,
                Cik = t.Cik,
                CompositeFigi = t.CompositeFigi,
                ShareClassFigi = t.ShareClassFigi,
                PhoneNumber = t.PhoneNumber,
                Address = t.Address,
                City = t.City,
                State = t.State,
                PostalCode = t.PostalCode,
                Description = t.Description,
                SicCode = t.SicCode,
                SicDescription = t.SicDescription,
                TickerRoot = t.TickerRoot,
                HomepageUrl = t.HomepageUrl,
                TotalEmployees = t.TotalEmployees,
                ListDate = t.ListDate,
                LogoUrl = t.LogoUrl,
                IconUrl = t.IconUrl,
                ShareClassSharesOutstanding = t.ShareClassSharesOutstanding,
                WeightedSharesOutstanding = t.WeightedSharesOutstanding,
                RoundLot = t.RoundLot,
                Ticker = t.Ticker
            });
            return Ok(result);
        }

        [Authorize]
        [HttpPost("{login}/subs/subscribe")]
        public async Task<IActionResult> AddSub(string login, string ticker)
        {
            var r = await _usersRepository.AddSubscriptionAsync(login, ticker);
            if (r == null) return Conflict($"api/users/{login}/subs");
            var result = r.Select(t => new TickerDTO
            {
               Name = t.Name,
               Market = t.Market,
               Locale = t.Locale,
               PrimaryExchange = t.PrimaryExchange,
               Type = t.Type,
               Active = t.Active,
               CurrencyName = t.CurrencyName,
               Cik = t.Cik,
               CompositeFigi = t.CompositeFigi,
               ShareClassFigi = t.ShareClassFigi,
               PhoneNumber = t.PhoneNumber,
               Address = t.Address,
               City = t.City,
               State = t.State,
               PostalCode = t.PostalCode,
               Description  = t.Description,   
               SicCode = t.SicCode,
               SicDescription = t.SicDescription,
               TickerRoot = t.TickerRoot,
               HomepageUrl = t.HomepageUrl,
               TotalEmployees = t.TotalEmployees,
               ListDate = t.ListDate,
               LogoUrl = t.LogoUrl,
               IconUrl = t.IconUrl,
               ShareClassSharesOutstanding = t.ShareClassSharesOutstanding,
               WeightedSharesOutstanding = t.WeightedSharesOutstanding,
               RoundLot = t.RoundLot,
               Ticker = t.Ticker
            });
            return Created($"api/{login}/subs/", result);
        }

        [Authorize]
        [HttpDelete("{login}/subs/unsubscribe")]
        public async Task<IActionResult> RemoveSub(string login, string ticker)
        {
            var r = await _usersRepository.DeleteSubscriptionAsync(login, ticker);
            if (r)
                return Ok();
            else
                return NoContent();
        }
    }
}
