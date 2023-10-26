using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DataContext _context;
        public AdminRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> AdminExists(int adminId)
        {
            var adminExists = await _context.Admins.AnyAsync(a => a.Id == adminId); ;
            if (!adminExists)
            {
                throw new AdminNotFoundException($"Admin with ID {adminId} not found");
            }
            return true;
        }

        public async Task<bool> DeleteAdmin(Admin admin)
        {
            _context.Remove(admin);
            return await Save();
        }

        public async Task<Admin> GetAdminById(int adminId)
        {
            var adminInDb = await _context.Admins.Where(a => a.Id == adminId).FirstOrDefaultAsync();
            if (adminInDb is null)
                throw new AdminNotFoundException($"Admin with ID {adminId} not found");
            return adminInDb;
        }

        public async Task<Admin> GetAdminByUsername(string username)
        {
            var adminInDb = await _context.Admins.Where(a => a.Username == username).FirstOrDefaultAsync();
            if (adminInDb is null)
                throw new AdminNotFoundException($"Admin : {username} not found");
            return adminInDb;
        }

        public async Task<ICollection<Admin>> GetAdmins()
        {
            return await _context.Admins.OrderBy(a => a.Id).ToListAsync();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
    }
}