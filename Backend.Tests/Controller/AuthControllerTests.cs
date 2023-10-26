using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Controller
{
    public class AuthControllerTests
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly IAdminRepository _adminRepository;
        public AuthControllerTests()
        {
            _authRepository = A.Fake<IAuthRepository>();
            _mapper = A.Fake<IMapper>();
            _adminRepository = A.Fake<IAdminRepository>();
        }

        [Fact]
        public async Task AdminController_Register_ReturnOk()
        {
            // Arrange
            var adminCreated = new RegisterAdminDto
            {
                Username = "newadmin",
                Password = "123456",
                ConfirmPassword = "123456"
            };

            A.CallTo(() => _adminRepository.GetAdmins()).Returns(new List<Admin>());
            A.CallTo(() => _authRepository.Register(adminCreated)).Returns(true);

            var controller = new AuthController(_authRepository, _mapper, _adminRepository);

            // Act
            var result = await controller.Register(adminCreated);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AdminController_Register_ReturnBadRequestOnDuplicateUsername()
        {
            // Arrange
            var adminCreated = new RegisterAdminDto
            {
                Username = "newadmin",
                Password = "123456",
                ConfirmPassword = "123456"
            };

            var existingAdmins = new List<Admin>
            {
                new Admin
                {
                    Username = "existingadmin",
                    // Set other properties as needed
                }
            };

            A.CallTo(() => _adminRepository.GetAdmins()).Returns(existingAdmins);

            var controller = new AuthController(_authRepository, _mapper, _adminRepository);

            // Act
            var result = await controller.Register(adminCreated);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task AuthController_Register_ReturnInternalServerError()
        {
            // Arrange
            var adminCreated = new RegisterAdminDto
            {
                Username = "newadmin",
                Password = "123456",
                ConfirmPassword = "123456"
            };

            A.CallTo(() => _adminRepository.GetAdmins()).Returns(new List<Admin>());
            A.CallTo(() => _authRepository.Register(adminCreated)).Returns(false);

            var controller = new AuthController(_authRepository, _mapper, _adminRepository);

            // Act
            var result = await controller.Register(adminCreated);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task AdminController_Register_ReturnBadRequestOnInvalidModel()
        {
            // Arrange
            var adminCreated = new RegisterAdminDto
            {
                // Set properties with invalid values
            };

            var controller = new AuthController(_authRepository, _mapper, _adminRepository);
            controller.ModelState.AddModelError("Username", "Invalid field value");

            // Act
            var result = await controller.Register(adminCreated);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();
        }


        [Fact]
        public async Task AuthController_Login_ReturnOkWithToken()
        {
            // Arrange
            var adminLogged = new LoginAdminDto
            {
                Username = "validusername",
                Password = "validpassword"
            };

            A.CallTo(() => _authRepository.Login(adminLogged)).Returns("validToken");
            var controller = new AuthController(_authRepository, _mapper, _adminRepository);

            // Act
            var result = await controller.Login(adminLogged);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(new { Token = "validToken" });
        }

        [Fact]
        public async Task AuthController_Login_ReturnBadRequestOnInvalidRequest()
        {
            var adminLogged = new LoginAdminDto
            {
                Username = "invalidusername",
                Password = "invalidpassword"
            };
            //Arrange
            var controller = new AuthController(_authRepository, _mapper, _adminRepository);

            // Act
            var result = await controller.Login(adminLogged);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().Be("Authentication failed.");
        }

        [Fact]
        public async Task AuthController_Login_ReturnBadRequestOnAuthenticationFailure()
        {
            // Arrange
            var adminLogged = new LoginAdminDto
            {
                Username = "invalidusername",
                Password = "invalidpassword"
            };

            A.CallTo(() => _authRepository.Login(adminLogged)).Returns(string.Empty);
            var controller = new AuthController(_authRepository, _mapper, _adminRepository);

            // Act
            var result = await controller.Login(adminLogged);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().Be("Authentication failed.");
        }

        [Fact]
        public async Task AuthController_Login_ReturnInternalServerErrorOnException()
        {
            // Arrange
            var adminLogged = new LoginAdminDto
            {
                Username = "validusername",
                Password = "validpassword"
            };

            A.CallTo(() => _authRepository.Login(adminLogged)).Throws(new Exception("Simulated exception"));
            var controller = new AuthController(_authRepository, _mapper, _adminRepository);

            // Act
            var result = await controller.Login(adminLogged);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(500);
            result.Should().BeOfType<ObjectResult>().Which.Value.Should().Be("Simulated exception");
        }
    }
}