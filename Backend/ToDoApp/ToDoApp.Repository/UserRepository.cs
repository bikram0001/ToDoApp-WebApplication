using ToDoApp.Models.DTOModels;
using ToDoApp.Repository.Interfaces;
using ToDoApp.Repository.Models;

namespace ToDoApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ToDoAppContext _databaseContext;
        public UserRepository(ToDoAppContext dbContext)
        {
            _databaseContext = dbContext;
        }
        public void AddUser(UserDTO userDTO)
        {
            User user = Mapper.MapToUser(userDTO);
            _databaseContext.Users.Add(user);
            _databaseContext.SaveChanges();
        }
        public UserDTO IsUserExists(UserDTO userDTO)
        {
            User user = Mapper.MapToUser(userDTO);
            User userDetails = _databaseContext.Users.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);
            if (userDetails != null) { 
                return Mapper.MapToUserDTO(userDetails);
            }
            return null;
        }
        public bool IsUserNameExists(string userName) {
            return _databaseContext.Users.FirstOrDefault(user => user.UserName == userName) != null;
        }
        public UserDTO getUserDetails(int userId) {
            User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userId);
            return Mapper.MapToUserDTO(user);
        }
    }
}
