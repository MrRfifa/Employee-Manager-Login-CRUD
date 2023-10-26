using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Interfaces
{
    public interface IAdminRepository
    {
        Task<ICollection<Admin>> GetAdmins();
        Task<Admin> GetAdminById(int adminId);
        Task<Admin> GetAdminByUsername(string username);
        Task<bool> AdminExists(int adminId);
        Task<bool> DeleteAdmin(Admin admin);
        Task<bool> Save();
    }
}