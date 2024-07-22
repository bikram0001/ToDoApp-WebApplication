using ToDoApp.Models.DTOModels;
using ToDoApp.Models.ViewModels;

namespace ToDoApp.Services.Interfaces
{
    public interface ITaskContract
    {
        void AddTask(TaskRequest task,int userId);
        List<TaskResponse> GetAllTasks(int userId);
        List<TaskResponse> GetActiveTasks(int userId);
        List<TaskResponse> GetCompletedTasks(int userId);
        void DeleteTask(int userId, int id);
        void DeleteTasks(int userId);
        void ChangeTaskStatus(int userId, int id);
        void UpdateTask(TaskRequest taskDTO, int userId, int id);
        Kpi PerformanceIndicator(int userId);
    }
}
