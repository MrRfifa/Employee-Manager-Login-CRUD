using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Dto.Admins;
using Backend.Dto.Employees;
using Backend.Models;

namespace Backend.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Admin, GetAdminDto>();
            CreateMap<Employee, GetEmployeeDto>();
            CreateMap<AddEmployeeDto, Employee>();
            CreateMap<Admin, LoginAdminDto>();
            CreateMap<RegisterAdminDto, Admin>();
        }
    }
}