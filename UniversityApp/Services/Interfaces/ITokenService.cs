using UniversityApp.Models;
using UniversityManagament.Models;

namespace UniversityManagament.Services.Interfaces;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(User user);
}