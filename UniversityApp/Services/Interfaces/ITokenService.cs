using UniversityApp.Models;

namespace UniversityApp.Services.Interfaces;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(User user);
}