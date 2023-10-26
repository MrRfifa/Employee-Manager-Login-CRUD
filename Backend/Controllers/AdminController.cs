using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Dto.Admins;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;

        public AdminController(IAdminRepository adminRepository, IMapper mapper)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetAdminDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAdmins()
        {
            try
            {
                var admins = await _adminRepository.GetAdmins();
                var adminDtos = _mapper.Map<List<GetAdminDto>>(admins);

                return Ok(adminDtos);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Error", "An error occurred while retrieving admin data.");
                return BadRequest(ModelState);
            }
        }

        [HttpGet("{adminId}")]
        [ProducesResponseType(200, Type = typeof(GetAdminDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAdminById(int adminId)
        {
            try
            {
                if (!await _adminRepository.AdminExists(adminId))
                    return NotFound();
                var admin = await _adminRepository.GetAdminById(adminId);
                var adminDto = _mapper.Map<GetAdminDto>(admin);

                return Ok(adminDto);
            }
            catch (AdminNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Error", "An error occurred while retrieving admin data.");
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{adminId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAdmin(int adminId)
        {
            try
            {
                if (!await _adminRepository.AdminExists(adminId))
                    return NotFound();

                var adminToDelete = await _adminRepository.GetAdminById(adminId);

                if (adminToDelete == null)
                    return NotFound();

                if (!await _adminRepository.DeleteAdmin(adminToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong deleting Admin");
                    return BadRequest(ModelState);
                }
                return NoContent();
            }
            catch (AdminNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while deleting Admin.");
                return BadRequest(ModelState);
            }
        }

    }
}