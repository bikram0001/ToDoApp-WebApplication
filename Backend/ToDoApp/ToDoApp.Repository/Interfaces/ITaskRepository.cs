using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Models.DTOModels;
using ToDoApp.Repository.Models;

namespace ToDoApp.Repository.Interfaces
{
    public interface ITaskRepository
    {
        IEnumerable<TaskResponseDTO> GetTasks(int userId);
        IEnumerable<TaskResponseDTO> GetCompletedTasks(int userId);
        IEnumerable<TaskResponseDTO> GetActiveTasks(int userId);
        void AddTask(TaskRequestDTO task, int userId);
        void DeleteTask(int userId, int id);
        void DeleteTasks(int userId);
        void ChangeTaskStatus(int userId, int id);
        void UpdateTask(TaskRequestDTO task, int userId, int id);
        KpiDTO PerformanceIndicator(int userId);
    }
}
