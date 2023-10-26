using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Dto.Admins;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IAdminRepository _adminRepository;
        private readonly IConfiguration _configuration;
        public AuthRepository(DataContext context, IAdminRepository adminRepository, IConfiguration configuration)
        {
            _adminRepository = adminRepository;
            _context = context;
            _configuration = configuration;
        }
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA256())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public string CreateToken(Admin admin)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            //var cred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<string> Login(LoginAdminDto adminLogged)
        {

            try
            {
                var admin = await _adminRepository.GetAdminByUsername(adminLogged.Username);

                if (admin == null)
                {
                    throw new AdminNotFoundException("User not found.");
                }

                if (!VerifyPasswordHash(adminLogged.Password, admin.PasswordHash, admin.PasswordSalt))
                {
                    throw new Exception("Wrong password.");
                }

                string token = CreateToken(admin);

                return token;
            }
            catch (AdminNotFoundException ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Register(RegisterAdminDto adminCreated)
        {
            try
            {
                CreatePasswordHash(adminCreated.Password, out byte[] passwordHash, out byte[] passwordSalt);
                var adminEntity = new Admin
                {
                    Username = adminCreated.Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                };

                _context.Admins.Add(adminEntity);

                return await Save();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA256(passwordSalt))
            {
                passwordSalt = hmac.Key;
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}