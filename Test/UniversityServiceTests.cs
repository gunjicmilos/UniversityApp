using Moq;
using UniversityApp.Repository.IRepository;
using UniversityApp.Services;
using UniversityApp.Services.Interfaces;
using UniversityManagament.Models.Dto;

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

    [Fact]
    public async Task GetUniversities_ReturnsListOfUniversities()
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
    }
}