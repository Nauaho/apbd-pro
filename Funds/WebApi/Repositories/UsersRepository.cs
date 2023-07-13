using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Models;
using WebApi.Models.DTOs;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Data.SqlTypes;

namespace WebApi.Repositories
{
    public interface IUsersRepository
    {
        public Task<ICollection<TickerDetails>?> AddSubscriptionAsync(string login, string symbol);
        public Task<Models.RefreshToken?> AddUserAsync(RegisterOrLoginRequest rl);
        public Task<Models.RefreshToken?> CheckUserAsync(RegisterOrLoginRequest rl);
        public Task<bool> DeleteSubscriptionAsync(string login, string symbol);
        public Task<ICollection<TickerDetails>?> GetSubscriptionAsync(string login);
        public Task<string?> UpdateRefreshTokenAsync(string RefreshToken);
    }
    public class UsersRepository : IUsersRepository
    {
        private readonly ProContext _context;
        private readonly PasswordHasher<User> _hasher;

        public UsersRepository(ProContext context, PasswordHasher<User> hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        public async Task<Models.RefreshToken?> AddUserAsync(RegisterOrLoginRequest rl)
        {
            if (await _context.Users.AnyAsync(u => u.Login == rl.Login || u.Email == rl.Email))
                return null;
            var user = new User
            {
                Email = rl.Email ?? throw new NullReferenceException(),
                Login = rl.Login,
            };
            user.Password = _hasher.HashPassword(user, rl.Password);
            await _context.Users.AddAsync(user);
            var refreshToken = CreateRefreshToken(user);
            await _context.RefreshToken.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<string?> UpdateRefreshTokenAsync(string RefreshToken)
        {
            var session = GetSession(RefreshToken);
            Console.WriteLine("Session: "+session);
            if (session is null)
                return null;

            var invalidate = await _context.RefreshToken.FirstOrDefaultAsync(r => r.Session == session);
            Console.WriteLine("Invalidate: " + invalidate);
            if (invalidate is null)
                return null;

            var refreshToken = await _context.RefreshToken.FirstOrDefaultAsync(r => r.Token == RefreshToken);
            Console.WriteLine("RT: " + refreshToken);
            if (refreshToken == null) 
            {
                _context.RefreshToken.Remove(invalidate);
                await _context.SaveChangesAsync();
                return null;
            }

            refreshToken.Token = GenerateRefreshJWT(refreshToken.UserLogin, refreshToken.Session);
            Console.WriteLine(refreshToken.Token);
            refreshToken.Expiration = DateTime.Now.AddDays(5);
            await _context.SaveChangesAsync();
            return refreshToken.Token;
        }

        public async Task<Models.RefreshToken?> CheckUserAsync(RegisterOrLoginRequest rl)
        {
            var u = _context.Users.Where(a => a.Login == rl.Login || a.Email == rl.Email).FirstOrDefault();

            if (u == null)
                return null;
            if (u.Password == null)
                throw new NullReferenceException();

            var res = _hasher.VerifyHashedPassword(u, u.Password, rl.Password);
            if (res == PasswordVerificationResult.Success)
            {
                var a = CreateRefreshToken(u);
                await _context.RefreshToken.AddAsync(a);
                await _context.SaveChangesAsync();
                return a;
            }
            else if (res == PasswordVerificationResult.Failed)
            {
                return null;
            }
            else
            {
                u.Password = _hasher.HashPassword(u, u.Password);
                var a = CreateRefreshToken(u);
                await _context.RefreshToken.AddAsync(a);
                await _context.SaveChangesAsync();
                return a;
            }
        }

        private static string GenerateRefreshJWT(string login, string session)
        {
            var token = new JwtSecurityToken
            (
                audience: login,
                expires: DateTime.Now.AddDays(5),
                issuer: session
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private static string? GetSession(string token)
        {
            try
            {
                var jwt = new JwtSecurityToken (token);
                return jwt.Issuer;

            }catch(Exception) 
            {
                return null;
            }
        }

        private static Models.RefreshToken CreateRefreshToken(User user)
        {
            var a = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var refreshToken = new Models.RefreshToken()
            {
                Session = a,
                Token = GenerateRefreshJWT(user.Login, a),
                Expiration = DateTime.Now.AddDays(5),
                UserLogin = user.Login
            };
            return refreshToken;
        }

        public async Task<ICollection<TickerDetails>?> GetSubscriptionAsync(string login)
        {
            var usersMaybe =  _context.Users.Include(u => u.TickersWatching)
                                           .ThenInclude(t => t.Ticker)
                                           .Where(a => a.Login == login);
            if (!usersMaybe.Any())
                return null;
            var user = await usersMaybe.FirstAsync();
            var subs = user.TickersWatching
                           .Select(ts => ts.Ticker)
                           .ToList();
            return subs;
        }

        public async Task<ICollection<TickerDetails>?> AddSubscriptionAsync(string login, string symbol)
        {
            var a = await _context.TickerUser.Where(s => s.UserLogin == login && s.TickerSymbol == symbol).FirstOrDefaultAsync();
            if (a != null)
                return null;
            await _context.TickerUser.AddAsync( new TickerUser { UserLogin = login, TickerSymbol = symbol} );
            await _context.SaveChangesAsync();
            return await GetSubscriptionAsync(login);
        }

        public async Task<bool> DeleteSubscriptionAsync(string login, string symbol)
        {
            var a = await _context.TickerUser.Where(s => s.UserLogin == login && s.TickerSymbol == symbol).FirstOrDefaultAsync();
            if (a == null) 
                return false;
            _context.TickerUser.Remove(a);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
