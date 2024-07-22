using Microsoft.EntityFrameworkCore;
using ToDoApp.Models.DTOModels;
using ToDoApp.Repository.Enums;
using ToDoApp.Repository.Interfaces;
using ToDoApp.Repository.Models;

namespace ToDoApp.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ToDoAppContext _databaseContext;
        public TaskRepository(ToDoAppContext dbContext)
        {
            _databaseContext = dbContext;
        }
        public IEnumerable<TaskResponseDTO> GetTasks(int userId) {
            DateTime istTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            DateTime todayDate = istTime.Date;
            DateTime tomorrowDate = todayDate.AddDays(1);
            var tasks = _databaseContext.UserTasks.Include(userTasks => userTasks.Task)
                .Include(userTask => userTask.User)
                .Include(UserTask => UserTask.Status)
                .Where(userTask => userTask.UserId == userId && userTask.Flag==false && userTask.CreatedOn >= todayDate && userTask.CreatedOn < tomorrowDate).OrderBy(userTask => userTask.StatusId);
            List<TaskResponseDTO> tasksDTO = [];
            foreach (var task in tasks)
            {
                tasksDTO.Add(Mapper.MapToTaskResponseDTO(task));
            }
            return tasksDTO;
        }
        public IEnumerable<TaskResponseDTO> GetCompletedTasks(int userId) {
            DateTime istTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            DateTime todayDate = istTime.Date;
            DateTime tomorrowDate = todayDate.AddDays(1);
            var tasks = _databaseContext.UserTasks.Include(userTasks => userTasks.Task)
                .Include(userTask => userTask.User)
                .Include(UserTask => UserTask.Status)
                .Where(userTask => userTask.UserId == userId && userTask.Flag == false && userTask.Status.Id==(int)TaskState.Completed && userTask.CreatedOn >= todayDate &&
                  userTask.CreatedOn < tomorrowDate);
            List<TaskResponseDTO> tasksDTO = [];
            foreach (var task in tasks)
            {
                tasksDTO.Add(Mapper.MapToTaskResponseDTO(task));
            }
            return tasksDTO;
        }
        public IEnumerable<TaskResponseDTO> GetActiveTasks(int userId) {
            DateTime istTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            DateTime todayDate = istTime.Date;
            DateTime tomorrowDate = todayDate.AddDays(1);
            var tasks = _databaseContext.UserTasks.Include(userTasks => userTasks.Task)
                .Include(userTask => userTask.User)
                .Include(UserTask => UserTask.Status)
                .Where(userTask => userTask.UserId == userId && userTask.Flag == false && userTask.Status.Id == (int)TaskState.Active && userTask.CreatedOn >= todayDate && userTask.CreatedOn < tomorrowDate);
            List<TaskResponseDTO> tasksDTO = [];
            foreach (var task in tasks)
            {
                tasksDTO.Add(Mapper.MapToTaskResponseDTO(task));
            }
            return tasksDTO;
        }
        public void AddTask(TaskRequestDTO task,int userId) {
            TaskInfo taskEntity = Mapper.MapToTaskInfo(task);
            int taskId = GetTaskId(taskEntity);
            if (taskId==0) {
                _databaseContext.TaskInfos.Add(taskEntity);
                _databaseContext.SaveChanges();
                taskId = GetTaskId(taskEntity);
            }
            var a = GetTaskId(taskEntity);
            UserTask userTask = new UserTask()
            {
                UserId = userId,
                TaskId = taskId,
                StatusId = 1,
                CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.ToUniversalTime(), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"))
            };
            _databaseContext.UserTasks.Add(userTask);
            _databaseContext.SaveChanges();
        }
        public int GetTaskId(TaskInfo task) {
            int taskId = (from taskInfo in _databaseContext.TaskInfos
                            where taskInfo.Title == task.Title && taskInfo.Description == task.Description
                            select taskInfo.Id).FirstOrDefault();
            return taskId;
        }
        public void DeleteTask(int userId,int id) {
            var task = _databaseContext.UserTasks.FirstOrDefault(usertask => usertask.UserId == userId && usertask.Id == id);
            task.Flag = true;
            _databaseContext.SaveChanges();
        }
        public void DeleteTasks(int userId) {
            var tasks = _databaseContext.UserTasks.Where(usertask => usertask.UserId == userId);
            foreach (var task in tasks)
            {
                task.Flag = true;
            }
            _databaseContext.SaveChanges();
        }
        public void ChangeTaskStatus(int userId, int id) {
            var task = _databaseContext.UserTasks.FirstOrDefault(task => task.UserId==userId && task.Id==id);
            if (task != null) {
                if (task.StatusId == (int)TaskState.Active) {
                    task.StatusId = (int)TaskState.Completed;
                    task.CompletedOn = DateTime.Now;
                }
                else
                {
                    task.StatusId = (int)TaskState.Active;
                    task.CompletedOn = null;
                }
            }
            _databaseContext.SaveChanges();
        }
        public void UpdateTask(TaskRequestDTO task,int userId,int id) {
            TaskInfo taskEntity = Mapper.MapToTaskInfo(task);
            int taskId = GetTaskId(taskEntity);
            if (taskId == 0)
            {
                _databaseContext.TaskInfos.Add(taskEntity);
                _databaseContext.SaveChanges();
                taskId = GetTaskId(taskEntity);
            }
            var userTask = _databaseContext.UserTasks.FirstOrDefault( x => x.UserId == userId && x.Id==id);
            if (userTask!=null)
            {
                userTask.TaskId = taskId;
                _databaseContext.SaveChanges();
            }
        }
        public KpiDTO PerformanceIndicator(int userId) {
            var activeTasksCount = GetActiveTasks(userId).Count();
            var completedTasksCount = GetCompletedTasks(userId).Count();
            var Total = activeTasksCount + completedTasksCount;
            if (Total > 0) {
                return new KpiDTO
                {
                    ActiveTasks = (int)Math.Round((double)activeTasksCount / Total * 100),
                    CompletedTasks = (int)Math.Round((double)completedTasksCount / Total * 100)
                };
            }
            return new KpiDTO
            {
                ActiveTasks = 0,
                CompletedTasks = 0
            };
        }
    }
}
