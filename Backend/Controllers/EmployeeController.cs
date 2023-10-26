using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Dto.Employees;
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
    public class EmployeeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;

        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetEmployeeDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [AllowAnonymous]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var employees = await _employeeRepository.GetEmployees();
                var employeeDtos = _mapper.Map<List<GetEmployeeDto>>(employees);

                return Ok(employeeDtos);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Error", "An error occurred while retrieving data.");
                return BadRequest(ModelState);
            }
        }

        [HttpGet("{employeeId}")]
        [ProducesResponseType(200, Type = typeof(GetEmployeeDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetEmployeeById(int employeeId)
        {
            try
            {
                if (!await _employeeRepository.EmployeeExists(employeeId))
                    return NotFound();
                var employee = await _employeeRepository.GetEmployee(employeeId);
                var employeeDto = _mapper.Map<GetEmployeeDto>(employee);

                return Ok(employeeDto);
            }
            catch (EmployeeNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Error", "An error occurred while retrieving employee data.");
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{employeeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> DeleteEmployee(int employeeId)
        {
            try
            {
                if (!await _employeeRepository.EmployeeExists(employeeId))
                    return NotFound();

                var employeeToDelete = await _employeeRepository.GetEmployee(employeeId);

                if (employeeToDelete == null)
                    return NotFound();

                if (!await _employeeRepository.DeleteEmployee(employeeToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong deleting the employee");
                    return BadRequest(ModelState);
                }
                return NoContent();
            }
            catch (EmployeeNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while deleting Admin.");
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> CreateEmployee([FromBody] AddEmployeeDto employeeCreate)
        {
            if (employeeCreate == null)
                return BadRequest(ModelState);

            var employees = await _employeeRepository.GetEmployees();
            var filtredEmployees = employees.Where(c => c.Email.Trim().ToUpper() == employeeCreate.Email.TrimEnd().ToUpper())
                .FirstOrDefault();


            if (filtredEmployees != null)
            {
                ModelState.AddModelError("", "Employee already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employeeMap = _mapper.Map<Employee>(employeeCreate);

            if (!await _employeeRepository.CreateEmployee(employeeMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{employeeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateEmployee(int employeeId, [FromBody] AddEmployeeDto updatedEmployee)
        {
            if (updatedEmployee == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _employeeRepository.EmployeeExists(employeeId))
            {
                return NotFound(ModelState);
            }

            var existingEmployee = await _employeeRepository.GetEmployee(employeeId);

            if (existingEmployee == null)
            {
                return NotFound(ModelState);
            }

            // Apply updates from the DTO to the existing employee
            existingEmployee.FirstName = updatedEmployee.FirstName ?? existingEmployee.FirstName;
            existingEmployee.LastName = updatedEmployee.LastName ?? existingEmployee.LastName;
            existingEmployee.Email = updatedEmployee.Email ?? existingEmployee.Email;
            existingEmployee.PhoneNumber = updatedEmployee.PhoneNumber ?? existingEmployee.PhoneNumber;

            // Check if DateOfBirth in DTO is not null, and if so, update DateOfBirth
            if (updatedEmployee.DateOfBirth != existingEmployee.DateOfBirth)
            {
                existingEmployee.DateOfBirth = updatedEmployee.DateOfBirth;
            }

            if (!await _employeeRepository.UpdateEmployee(existingEmployee))
            {
                ModelState.AddModelError("", "Something went wrong updating employee");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}