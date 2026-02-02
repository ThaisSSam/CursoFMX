using ITB.Application.Dtos;

namespace ITB.Application.Interfaces
{
    public interface IUserService
    {
        Task RegisterAsync(RegisterUserRequest request);
        Task<string> LoginAsync(LoginRequest request);

    }
}
