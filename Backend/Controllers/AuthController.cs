using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Dto;
using Backend.Dto.Admins;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly IAdminRepository _adminRepository;

        public AuthController(IAuthRepository authRepository, IMapper mapper, IAdminRepository adminRepository)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _adminRepository = adminRepository;
        }

        [HttpPost("register")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] RegisterAdminDto adminCreated)
        {
            if (adminCreated == null)
                return BadRequest(ModelState);

            var admins = await _adminRepository.GetAdmins();
            var filteredAdmin = admins
                .Where(c => c.Username.Trim().ToUpper() == adminCreated.Username.Trim().ToUpper())
                .FirstOrDefault();
            if (filteredAdmin != null)
            {
                ModelState.AddModelError("", "Admin username already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var adminMap = _mapper.Map<Admin>(adminCreated);


            if (!await _authRepository.Register(adminCreated))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPost("login")]
        [ProducesResponseType(200)] // Add other response types as needed
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] LoginAdminDto adminLogged)
        {
            try
            {
                if (adminLogged == null)
                {
                    return BadRequest("Invalid request.");
                }

                var token = await _authRepository.Login(adminLogged);

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest("Authentication failed.");
                }

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}