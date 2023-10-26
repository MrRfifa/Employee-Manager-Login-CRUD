using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Dto.Admins;
using Backend.Models;

namespace Backend.Interfaces
{
    public interface IAuthRepository
    {
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        public string CreateToken(Admin admin);
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        public Task<string> Login(LoginAdminDto adminLogged);
        public Task<bool> Register(RegisterAdminDto adminCreated);
        public Task<bool> Save();
    }
}