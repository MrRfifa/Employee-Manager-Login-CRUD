
namespace Backend.Tests.Controller
{
    public class EmployeeControllerTests
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeControllerTests()
        {
            _employeeRepository = A.Fake<IEmployeeRepository>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public async Task EmployeeController_GetEmployees_ReturnOk()
        {
            // Arrange
            var employees = A.Fake<ICollection<Employee>>();
            var employeesList = A.Fake<List<GetEmployeeDto>>();
            A.CallTo(() => _employeeRepository.GetEmployees()).Returns(employees);
            A.CallTo(() => _mapper.Map<List<GetEmployeeDto>>(employees)).Returns(employeesList);
            var controller = new EmployeeController(_employeeRepository, _mapper);
            // Act
            var result = await controller.GetEmployees();
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task EmployeeController_GetEmployeeById_ReturnOk()
        {

            // Arrange
            int employeeId = 1;
            var employee = A.Fake<Employee>();
            var employeeDto = A.Fake<GetEmployeeDto>();
            A.CallTo(() => _employeeRepository.EmployeeExists(employeeId)).Returns(true);
            A.CallTo(() => _employeeRepository.GetEmployee(employeeId)).Returns(employee);
            A.CallTo(() => _mapper.Map<GetEmployeeDto>(employee)).Returns(employeeDto);
            var controller = new EmployeeController(_employeeRepository, _mapper);
            // Act
            var result = await controller.GetEmployeeById(employeeId);
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task EmployeeController_GetEmployeeById_ReturnNotFound()
        {
            // Arrange
            int employeeId = 1;
            A.CallTo(() => _employeeRepository.EmployeeExists(employeeId)).Returns(false);
            var controller = new EmployeeController(_employeeRepository, _mapper);
            // Act
            var result = await controller.GetEmployeeById(employeeId);
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task EmployeeController_DeleteEmployee_ReturnNoContent()
        {

            // Arrange
            int employeeId = 1;
            var employeeToDelete = A.Fake<Employee>();
            A.CallTo(() => _employeeRepository.EmployeeExists(employeeId)).Returns(true);
            A.CallTo(() => _employeeRepository.GetEmployee(employeeId)).Returns(employeeToDelete);
            A.CallTo(() => _employeeRepository.DeleteEmployee(employeeToDelete)).Returns(true);
            var controller = new EmployeeController(_employeeRepository, _mapper);
            // Act
            var result = await controller.DeleteEmployee(employeeId);
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task EmployeeController_DeleteEmployee_ReturnNotFound()
        {

            // Arrange
            int employeeId = 1;
            A.CallTo(() => _employeeRepository.EmployeeExists(employeeId)).Returns(false);
            var controller = new EmployeeController(_employeeRepository, _mapper);
            // Act
            var result = await controller.DeleteEmployee(employeeId);
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task EmployeeController_DeleteEmployee_ReturnBadRequest()
        {

            // Arrange
            int employeeId = 1;
            var employeeToDelete = A.Fake<Employee>();
            A.CallTo(() => _employeeRepository.EmployeeExists(employeeId)).Returns(true);
            A.CallTo(() => _employeeRepository.GetEmployee(employeeId)).Returns(employeeToDelete);
            A.CallTo(() => _employeeRepository.DeleteEmployee(employeeToDelete)).Returns(false);
            var controller = new EmployeeController(_employeeRepository, _mapper);
            // Act
            var result = await controller.DeleteEmployee(employeeId);
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task EmployeeController_CreateEmployee_ReturnOk()
        {

            // Arrange
            var employeeCreateDto = new AddEmployeeDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                DateOfBirth = new DateTime(2008, 5, 1, 8, 30, 52)
            };

            var existingEmployees = new List<Employee>
            { };

            A.CallTo(() => _employeeRepository.GetEmployees()).Returns(existingEmployees);
            A.CallTo(() => _mapper.Map<Employee>(employeeCreateDto)).Returns(new Employee
            {

            });
            A.CallTo(() => _employeeRepository.CreateEmployee(A<Employee>._)).Returns(true);

            var controller = new EmployeeController(_employeeRepository, _mapper);
            // Act
            var result = await controller.CreateEmployee(employeeCreateDto);
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task EmployeeController_CreateEmployee_ReturnBadRequestOnDuplicate()
        {
            // Arrange
            var employeeCreateDto = new AddEmployeeDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                DateOfBirth = new DateTime(2008, 5, 1, 8, 30, 52)
            };

            var existingEmployees = new List<Employee>
            {
                new Employee
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    PhoneNumber = "1234567890",
                    DateOfBirth = new DateTime(2008, 5, 1, 8, 30, 52)
                }
            };

            A.CallTo(() => _employeeRepository.GetEmployees()).Returns(existingEmployees);

            var controller = new EmployeeController(_employeeRepository, _mapper);

            // Act
            var result = await controller.CreateEmployee(employeeCreateDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(422);
        }

        [Fact]
        public async Task EmployeeController_UpdateEmployee_ReturnOk()
        {

            // Arrange
            int employeeId = 1;
            var updatedEmployeeDto = new AddEmployeeDto
            {
                FirstName = "hana",
                LastName = "hana",
                Email = "hana.hana@hana.hana",
                PhoneNumber = "0123456789",
                DateOfBirth = new DateTime(1978, 5, 1, 8, 30, 52)
            };
            var existingEmployee = new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                DateOfBirth = new DateTime(2008, 5, 1, 8, 30, 52)
            };
            A.CallTo(() => _employeeRepository.EmployeeExists(employeeId)).Returns(true);
            A.CallTo(() => _employeeRepository.GetEmployee(employeeId)).Returns(existingEmployee);
            A.CallTo(() => _employeeRepository.UpdateEmployee(existingEmployee)).Returns(true);
            var controller = new EmployeeController(_employeeRepository, _mapper);
            // Act
            var result = await controller.UpdateEmployee(employeeId, updatedEmployeeDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task EmployeeController_UpdateEmployee_ReturnBadRequestOnInvalidModel()
        {
            // Arrange
            int employeeId = 1; // Replace with a valid employee ID
            var updatedEmployeeDto = new AddEmployeeDto
            {
                FirstName = "hana",
                LastName = "hana",
                Email = "hana.hana@hana.hana",
                PhoneNumber = "cwsv",
                DateOfBirth = new DateTime(1978, 5, 1, 8, 30, 52)
            };

            var controller = new EmployeeController(_employeeRepository, _mapper);
            controller.ModelState.AddModelError("PhoneNumber", "Invalid field value");

            // Act
            var result = await controller.UpdateEmployee(employeeId, updatedEmployeeDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task EmployeeController_UpdateEmployee_ReturnNotFoundWhenEmployeeNotExists()
        {
            // Arrange
            int employeeId = 1;
            var updatedEmployeeDto = new AddEmployeeDto
            {
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                Email = "updated.email@example.com",
            };

            A.CallTo(() => _employeeRepository.EmployeeExists(employeeId)).Returns(false);
            var controller = new EmployeeController(_employeeRepository, _mapper);

            // Act
            var result = await controller.UpdateEmployee(employeeId, updatedEmployeeDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task EmployeeController_UpdateEmployee_ReturnNotFoundWhenEmployeeIsNull()
        {
            // Arrange
            int employeeId = 1;
            var updatedEmployeeDto = new AddEmployeeDto
            {
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                Email = "updated.email@example.com",
            };

            A.CallTo(() => _employeeRepository.EmployeeExists(employeeId)).Returns(false);
            var controller = new EmployeeController(_employeeRepository, _mapper);

            // Act
            var result = await controller.UpdateEmployee(employeeId, updatedEmployeeDto);

            // Assert
            result.Should().NotBeNull().And.BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task EmployeeController_UpdateEmployee_ReturnInternalServerError(){

             // Arrange
            int employeeId = 1; 
            var updatedEmployeeDto = new AddEmployeeDto
            {
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                Email = "updated.email@example.com",
            };

            var existingEmployee = new Employee
            {
                FirstName = "OriginalFirstName",
                LastName = "OriginalLastName",
                Email = "original.email@example.com",
            };

            A.CallTo(() => _employeeRepository.EmployeeExists(employeeId)).Returns(true);
            A.CallTo(() => _employeeRepository.GetEmployee(employeeId)).Returns(existingEmployee);
            A.CallTo(() => _employeeRepository.UpdateEmployee(A<Employee>._)).Returns(false);
            var controller = new EmployeeController(_employeeRepository, _mapper);

            // Act
            var result = await controller.UpdateEmployee(employeeId, updatedEmployeeDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
        }

    }
}
