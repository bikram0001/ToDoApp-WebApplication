using ToDoApp.Models.DTOModels;
using ToDoApp.Repository.Models;

namespace ToDoApp.Repository
{
    public class Mapper
    {
        public static TaskInfo MapToTaskInfo(TaskRequestDTO dto) {
            return new TaskInfo
            {
                Title = dto.Title,
                Description = dto.Description
            };
        }
        public static TaskResponseDTO MapToTaskResponseDTO(UserTask task) {
            return new TaskResponseDTO
            {
                Id = task.Id,
                TaskId = task.TaskId,
                Title = task.Task.Title,
                Description = task.Task.Description,
                Status = task.Status.StatusName,
                CreatedOn = (DateTime)task.CreatedOn,
                CompletedOn = task.CompletedOn
            };
        }
        public static User MapToUser(UserDTO dto) {
            return new User
            {
                UserName = dto.UserName,
                Password = dto.Password
            };
        }
        public static UserDTO MapToUserDTO(User user) {
            return new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = user.Password
            };
        }
    }
}
