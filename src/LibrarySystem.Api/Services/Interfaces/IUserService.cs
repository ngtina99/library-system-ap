using LibrarySystem.Api.DTOs.Users;

namespace LibrarySystem.Api.Services.Interfaces;

public interface IUserService
{
    Task<UserResponse> CreateAsync(CreateUserRequest request);

    Task<List<UserResponse>> GetAllAsync();
}