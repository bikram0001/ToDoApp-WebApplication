using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Models.DTOModels;
using ToDoApp.Repository.Models;

namespace ToDoApp.Repository.Interfaces
{
    public interface IUserRepository
    {
        void AddUser(UserDTO user);
        UserDTO IsUserExists(UserDTO user);
        bool IsUserNameExists(string userName);
        UserDTO getUserDetails(int userId);
    }
}
