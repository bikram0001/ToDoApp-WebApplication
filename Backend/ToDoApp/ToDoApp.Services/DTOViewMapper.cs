using ToDoApp.Models.DTOModels;
using ToDoApp.Models.ViewModels;

namespace ToDoApp.Services
{
    public class DTOViewMapper
    {
        public static TaskRequestDTO MapToTaskRequestDTO(TaskRequest entity) {
            return new TaskRequestDTO
            {
                Title = entity.Title,
                Description = entity.Description
            };
        }
        public static TaskResponse MapToTaskResponse(TaskResponseDTO dto) {
            return new TaskResponse
            {
                Id = dto.Id,
                TaskId = dto.TaskId,
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                CreatedOn = dto.CreatedOn,
                CompletedOn = dto.CompletedOn
            };
        }
        public static UserDTO MapToUserDTO(User user) {
            return new UserDTO
            {
                UserName = user.UserName,
                Password = user.Password
            };
        }
        public static User MaptoUser(UserDTO dto) {
            return new User
            {
                UserName = dto.UserName,
                Password = dto.Password
            };
        }
        public static Kpi MapToKpi(KpiDTO dto) {
            return new Kpi
            {
                ActiveTasks = dto.ActiveTasks,
                CompletedTasks = dto.CompletedTasks
            };
        }
    }
}
