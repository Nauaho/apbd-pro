using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Models;
using WebApi.Models.DTOs;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Repositories
{
    public interface IUsersRepository
    {
        public Task<User?> AddUserAsync(RegisterOrLoginRequest rl);
        public Task<bool> CheckUserAsync(RegisterOrLoginRequest rl);
        public Task<bool> UpdateRefreshtokenAsync(string RefreshToken);
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

        public async Task<User?> AddUserAsync(RegisterOrLoginRequest rl)
        {
            if (await _context.Users.AnyAsync(u => u.Login == rl.Login || u.Email == rl.Email))
                return null;
            var user = new User
            {
                Email = rl.Email,
                Login = rl.Login,
                RefreshToken = GenerateRefreshJWT(rl.Login),
            };
            user.Password = _hasher.HashPassword(user, rl.Password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateRefreshtokenAsync(string RefreshToken)
        {
            var u = await _context.Users.Where(u => u.RefreshToken == RefreshToken).FirstOrDefaultAsync();
            if (u == null)
                return false;
            if (DateTime.Now > u.RefreshTokenExp)
            {
                u.RefreshToken = GenerateRefreshJWT(u.Login);
                u.RefreshTokenExp = DateTime.Now.AddDays(5);
            }
            return true;
        }

        public async Task<bool> CheckUserAsync(RegisterOrLoginRequest rl)
        {
            var u = _context.Users.Where(a => a.Login == rl.Login || a.Email == rl.Email).FirstOrDefault();

            if (u == null)
                return false;
            if (u.Password == null)
                throw new NullReferenceException();

            var res = _hasher.VerifyHashedPassword(u, u.Password, rl.Password);
            if (res == PasswordVerificationResult.Success)
            {
                return true;
            }
            else if (res == PasswordVerificationResult.Failed)
            {
                return false;
            }
            else
            {
                u.Password = _hasher.HashPassword(u, u.Password);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        private static string GenerateRefreshJWT(string login)
        {
            var token = new JwtSecurityToken
            (
                audience: login,
                expires: DateTime.Now.AddMinutes(5)
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
