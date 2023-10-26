using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Controller
{
    public class AdminControllerTests
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;

        public AdminControllerTests()
        {
            _adminRepository = A.Fake<IAdminRepository>();
            _mapper = A.Fake<IMapper>();

        }
        [Fact]
        public async Task AdminController_GetAdmins_ReturnOk()
        {
            // Arrange
            var admins = A.Fake<ICollection<Admin>>();
            var adminsList = A.Fake<List<GetAdminDto>>();
            A.CallTo(() => _adminRepository.GetAdmins()).Returns(admins);
            A.CallTo(() => _mapper.Map<List<GetAdminDto>>(admins)).Returns(adminsList);
            var controller = new AdminController(_adminRepository, _mapper);
            // Act
            var result = await controller.GetAdmins();
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }


        [Fact]
        public async Task AdminController_GetAdminById_ReturnOk()
        {

            // Arrange
            int adminId = 1;
            var admin = A.Fake<Admin>();
            var adminDto = A.Fake<GetAdminDto>();
            A.CallTo(() => _adminRepository.AdminExists(adminId)).Returns(true);
            A.CallTo(() => _adminRepository.GetAdminById(adminId)).Returns(admin);
            A.CallTo(() => _mapper.Map<GetAdminDto>(admin)).Returns(adminDto);
            var controller = new AdminController(_adminRepository, _mapper);
            // Act
            var result = await controller.GetAdminById(adminId);
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AdminController_GetAdminById_ReturnNotFound()
        {
            // Arrange
            int adminId = 1;
            A.CallTo(() => _adminRepository.AdminExists(adminId)).Returns(false);
            var controller = new AdminController(_adminRepository, _mapper);
            // Act
            var result = await controller.GetAdminById(adminId);
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task AdminController_DeleteAdmin_ReturnNoContent()
        {

            // Arrange
            int adminId = 1;
            var adminToDelete = A.Fake<Admin>();
            A.CallTo(() => _adminRepository.AdminExists(adminId)).Returns(true);
            A.CallTo(() => _adminRepository.GetAdminById(adminId)).Returns(adminToDelete);
            A.CallTo(() => _adminRepository.DeleteAdmin(adminToDelete)).Returns(true);
            var controller = new AdminController(_adminRepository, _mapper);
            // Act
            var result = await controller.DeleteAdmin(adminId);
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task AdminController_DeleteAdmin_ReturnNotFound()
        {

            // Arrange
            int adminId = 1;
            A.CallTo(() => _adminRepository.AdminExists(adminId)).Returns(false);
            var controller = new AdminController(_adminRepository, _mapper);
            // Act
            var result = await controller.DeleteAdmin(adminId);
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task AdminController_DeleteAdmin_ReturnBadRequest()
        {

            // Arrange
            int adminId = 1;
            var adminToDelete = A.Fake<Admin>();
            A.CallTo(() => _adminRepository.AdminExists(adminId)).Returns(true);
            A.CallTo(() => _adminRepository.GetAdminById(adminId)).Returns(adminToDelete);
            A.CallTo(() => _adminRepository.DeleteAdmin(adminToDelete)).Returns(false);
            var controller = new AdminController(_adminRepository, _mapper);
            // Act
            var result = await controller.DeleteAdmin(adminId);
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BadRequestObjectResult>();
        }

    }
}