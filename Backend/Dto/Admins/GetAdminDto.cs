using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dto.Admins
{
    public class GetAdminDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}