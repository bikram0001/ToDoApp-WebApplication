using ToDoApp.Models.DTOModels;
using ToDoApp.Models.ViewModels;
using ToDoApp.Repository.Interfaces;
using ToDoApp.Services.Interfaces;

namespace ToDoApp.Services.TaskManagement
{
    public class TaskServices : ITaskContract
    {
        private readonly ITaskRepository _taskRepository;
        public TaskServices(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public void AddTask(TaskRequest task, int userId) {
            TaskRequestDTO taskDTO = DTOViewMapper.MapToTaskRequestDTO(task);
            _taskRepository.AddTask(taskDTO,userId);
        }
        public List<TaskResponse> GetAllTasks(int userId) {
            var tasks = _taskRepository.GetTasks(userId);
            List <TaskResponse> tasksList = [];
            foreach (var task in tasks)
            {
                tasksList.Add(DTOViewMapper.MapToTaskResponse(task));
            }
            return tasksList;
        }
        public List<TaskResponse> GetActiveTasks(int userId) {
            var tasks = _taskRepository.GetActiveTasks(userId);
            List<TaskResponse> tasksList = [];
            foreach (var task in tasks)
            {
                tasksList.Add(DTOViewMapper.MapToTaskResponse(task));
            }
            return tasksList;
        }
        public List<TaskResponse> GetCompletedTasks(int userId) {
            var tasks = _taskRepository.GetCompletedTasks(userId);
            List<TaskResponse> tasksList = [];
            foreach (var task in tasks)
            {
                tasksList.Add(DTOViewMapper.MapToTaskResponse(task));
            }
            return tasksList;
        }
        public void DeleteTasks(int userId) { 
            _taskRepository.DeleteTasks(userId);
        }
        public void DeleteTask(int userId, int id) {
            _taskRepository.DeleteTask(userId, id);
        }
        public void ChangeTaskStatus(int userId, int id) {
            _taskRepository.ChangeTaskStatus(userId, id);
        }
        public void UpdateTask(TaskRequest task, int userId, int id) {
            TaskRequestDTO taskDTO = DTOViewMapper.MapToTaskRequestDTO(task);
            _taskRepository.UpdateTask(taskDTO, userId, id);
        }
        public Kpi PerformanceIndicator(int userId) {
            KpiDTO dto = _taskRepository.PerformanceIndicator(userId);
            return DTOViewMapper.MapToKpi(dto);
        }
    }
}
