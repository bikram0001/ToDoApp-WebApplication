using ToDoApp.Models.DTOModels;
using ToDoApp.Models.ViewModels;

namespace ToDoApp.Services.Interfaces
{
    public interface IUserContract
    {
        void AddUser(User user);
        object? Login(User user);
        UserDTO IsUserExists(User user);
        bool IsUserNameExists(string userName);
        object? GenerateTokensFromRefreshToken(string refreshToken);
        UserDTO getUserDetails(int userId);
    }
}
