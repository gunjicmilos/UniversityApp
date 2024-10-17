using Moq;
using UniversityApp.Models;
using UniversityApp.Repository.IRepository;
using UniversityApp.Services;
using UniversityApp.Services.Interfaces;
using UniversityManagament.Models.Dto;
using Xunit;

namespace Test;

public class UniversityServiceTests
{
    private readonly Mock<IUniversityRepository> _mockRepo;
    private readonly IUniversityService _universityService;

    public UniversityServiceTests()
    {
        _mockRepo = new Mock<IUniversityRepository>();
        _universityService = new UniversityService(_mockRepo.Object);
    }

    //[Fact]
    /*public async Task GetUniversities_ReturnsListOfUniversities()
    {
        // Arrange
        var expectedUniversities = new List<UniversityDto>
        {
            new UniversityDto { Id = Guid.NewGuid(), Name = "Test University", Location = "Test Location" }
        };

        _mockRepo.Setup(repo => repo.GetAllUniversityAsync()).ReturnsAsync(expectedUniversities);

        // Act
        var result = await _universityService.GetUniversities();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUniversities.Count, result.Count);
    }*/
    
    // Test za GetUniversities
    [Fact]
    public async Task GetUniversities_ReturnsListOfUniversities()
    {
        // Arrange
        var universities = new List<UniversityDto>
        {
            new UniversityDto { Id = Guid.NewGuid(), Name = "Test University 1", Location = "Test Location 1" },
            new UniversityDto { Id = Guid.NewGuid(), Name = "Test University 2", Location = "Test Location 2" }
        };

        _mockRepo.Setup(repo => repo.GetAllUniversityAsync()).ReturnsAsync(universities);

        // Act
        var result = await _universityService.GetUniversities();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    // Test za GetUniversitiy sa validnim ID-om
    [Fact]
    public async Task GetUniversity_ExistingUniversity_ReturnsUniversity()
    {
        // Arrange
        var universityId = Guid.NewGuid();
        var university = new UniversityDto { Id = universityId, Name = "Test University", Location = "Test Location" };

        _mockRepo.Setup(repo => repo.GetUniversityAsync(universityId)).ReturnsAsync(university);

        // Act
        var result = await _universityService.GetUniversitiy(universityId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(universityId, result.Id);
        Assert.Equal("Test University", result.Name);
    }

    // Test za GetUniversitiy sa nepostojećim ID-om
    [Fact]
    public async Task GetUniversity_NonExistingUniversity_ReturnsNull()
    {
        // Arrange
        var universityId = Guid.NewGuid();

        _mockRepo.Setup(repo => repo.GetUniversityAsync(universityId)).ReturnsAsync((UniversityDto)null);

        // Act
        var result = await _universityService.GetUniversitiy(universityId);

        // Assert
        Assert.Null(result);
    }

    // Test za CreateUniversity sa novim univerzitetom
    [Fact]
    public async Task CreateUniversity_NewUniversity_ReturnsUniversityDto()
    {
        // Arrange
        var createDto = new CreateUniversityDto { Name = "New University", Location = "New Location" };
        var newUniversity = new University { Id = Guid.NewGuid(), Name = createDto.Name, Location = createDto.Location };

        _mockRepo.Setup(repo => repo.UniversityExistsAsync(createDto.Name)).ReturnsAsync((University)null);
        _mockRepo.Setup(repo => repo.CreateUniversityAsync(It.IsAny<University>()));

        // Act
        var result = await _universityService.CreateUniversity(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New University", result.Name);
    }
    
    // Test za UpdateUniversity sa postojećim univerzitetom
    [Fact]
    public async Task UpdateUniversity_ExistingUniversity_UpdatesSuccessfully()
    {
        // Arrange
        var universityId = Guid.NewGuid();
        var updateDto = new CreateUniversityDto { Name = "Updated University", Location = "Updated Location" };
        var existingUniversity = new University { Id = universityId, Name = "Old University", Location = "Old Location" };

        _mockRepo.Setup(repo => repo.FindUniversityAsync(universityId)).ReturnsAsync(existingUniversity);
        _mockRepo.Setup(repo => repo.UpdateUniversityAsync(universityId, existingUniversity));

        // Act
        var result = await _universityService.UpdateUniversitiy(universityId, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated University", result.Name);
        Assert.Equal("Updated Location", result.Location);
    }

    // Test za UpdateUniversity sa nepostojećim univerzitetom
    [Fact]
    public async Task UpdateUniversity_NonExistingUniversity_ReturnsNull()
    {
        // Arrange
        var universityId = Guid.NewGuid();
        var updateDto = new CreateUniversityDto { Name = "Updated University", Location = "Updated Location" };

        _mockRepo.Setup(repo => repo.FindUniversityAsync(universityId)).ReturnsAsync((University)null);

        // Act
        var result = await _universityService.UpdateUniversitiy(universityId, updateDto);

        // Assert
        Assert.Null(result);
    }

    // Test za DeleteUniversity sa postojećim univerzitetom
    [Fact]
    public async Task DeleteUniversity_ExistingUniversity_DeletesSuccessfully()
    {
        // Arrange
        var universityId = Guid.NewGuid();
        var existingUniversity = new University { Id = universityId, Name = "University to Delete", Location = "Location" };

        _mockRepo.Setup(repo => repo.FindUniversityAsync(universityId)).ReturnsAsync(existingUniversity);
        _mockRepo.Setup(repo => repo.DeleteUniversityAsync(existingUniversity));

        // Act
        var result = await _universityService.DeleteUniversitiy(universityId);

        // Assert
        Assert.NotNull(result);
    }

    // Test za DeleteUniversity sa nepostojećim univerzitetom
    [Fact]
    public async Task DeleteUniversity_NonExistingUniversity_ReturnsNull()
    {
        // Arrange
        var universityId = Guid.NewGuid();

        _mockRepo.Setup(repo => repo.FindUniversityAsync(universityId)).ReturnsAsync((University)null);

        // Act
        var result = await _universityService.DeleteUniversitiy(universityId);

        // Assert
        Assert.Null(result);
    }
}
