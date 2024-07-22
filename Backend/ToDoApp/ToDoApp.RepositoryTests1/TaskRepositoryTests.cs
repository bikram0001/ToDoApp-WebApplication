using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ToDoApp.Models.DTOModels;
using ToDoApp.Repository.Enums;
using ToDoApp.Repository.Models;

namespace ToDoApp.Repository.Tests
{
    [TestClass]
    public class TaskRepositoryTests
    {
        private Mock<ToDoAppContext> _mockContext;
        private TaskRepository _taskRepository;
        [TestInitialize]
        public void TestInitialize()
        {
            _mockContext = new Mock<ToDoAppContext>();
            _taskRepository = new TaskRepository(_mockContext.Object);
        }

        [TestMethod]
        public void GetTasks_Returns_TaskResponseDTO_List()
        {
            int userId = 1;
            var userTasks = new List<UserTask>
            {
               new UserTask { Id = 1, UserId = userId, TaskId = 1, CreatedOn = DateTime.UtcNow, StatusId = 1, Flag = false, Task = new TaskInfo { Id = 1, Title = "Task 1", Description = "Description 1" }, Status = new Status { Id = 1 } }
            }.AsQueryable();
            _mockContext.Setup(c => c.UserTasks).Returns(MockDbSet(userTasks));
            var result = _taskRepository.GetTasks(userId);
            Assert.IsNotNull(result);
            Assert.AreEqual(userTasks.Count(), result.Count());
        }

        [TestMethod]
        public void GetCompletedTasks_Returns_TaskResponseDTO_List()
        {
            int userId = 1;
            var completedTasks = new List<UserTask>
            {
                new UserTask { Id = 1, UserId = userId, TaskId = 1, CreatedOn = DateTime.UtcNow, StatusId = 1, Flag = false, Task = new TaskInfo { Id = 1, Title = "Task 1", Description = "Description 1" }, User = new User { Id = userId }, Status = new Status { Id = 2 } }
            }.AsQueryable();
            _mockContext.Setup(c => c.UserTasks).Returns(MockDbSet(completedTasks));
            var result = _taskRepository.GetCompletedTasks(userId);
            Assert.AreEqual(completedTasks.Count(), result.Count());
        }

        [TestMethod]
        public void GetActiveTasks_Returns_TaskResponseDTO_List()
        {
            int userId = 1;
            var activeTasks = new List<UserTask>
            {
                new UserTask { Id = 1, UserId = userId, TaskId = 1, CreatedOn = DateTime.UtcNow, StatusId = 1, Flag = false, Task = new TaskInfo { Id = 1, Title = "Task 1", Description = "Description 1" }, User = new User { Id = userId }, Status = new Status { Id = 1 } }
            }.AsQueryable();
            _mockContext.Setup(c => c.UserTasks).Returns(MockDbSet(activeTasks));
            var result = _taskRepository.GetActiveTasks(userId);
            Assert.AreEqual(activeTasks.Count(), result.Count());
        }

        [TestMethod]
        public void AddTask_WhenTaskIdIsZero_ShouldAddTaskInfosAndUserTasks()
        {
            var taskInfos = new List<TaskInfo>();
            _mockContext.Setup(c => c.TaskInfos).Returns(MockDbSet(taskInfos));
            var userTasks = new List<UserTask>();
            _mockContext.Setup(c => c.UserTasks).Returns(MockDbSet(userTasks));
            var taskRequestDTO = new TaskRequestDTO
            {
                Title = "Sample Task",
                Description = "Task Description"
            };
            int userId = 1;
            _mockContext.Setup(c => c.TaskInfos.Add(It.IsAny<TaskInfo>())).Callback<TaskInfo>(taskInfos.Add);
            _mockContext.Setup(c => c.UserTasks.Add(It.IsAny<UserTask>())).Callback<UserTask>(userTasks.Add);
            _mockContext.Setup(c => c.SaveChanges()).Returns(1);
            _taskRepository.AddTask(taskRequestDTO, userId);
            Assert.AreEqual(1, taskInfos.Count);
            Assert.AreEqual(1, userTasks.Count);
            _mockContext.Verify(c => c.SaveChanges(), Times.Exactly(2));
        }

        [TestMethod]
        public void GetTaskId_Returns_Correct_TaskId()
        {
            var task = new TaskInfo { Id = 1 ,Title = "Task 1", Description = "Description for Task 1" };
            var taskInfos = new List<TaskInfo> { task }.AsQueryable();

            _mockContext.Setup(c => c.TaskInfos).Returns(MockDbSet(taskInfos));
            var result = _taskRepository.GetTaskId(task);
            Assert.AreEqual(task.Id, result);
        }

        [TestMethod]
        public void DeleteTask_Sets_Flag_To_True_And_Saves_Changes()
        {
            int userId = 1;
            int Id = 1;
            var userTask = new UserTask
            {
                UserId = userId,
                Id = Id,
                Flag = false
            };
            var mockSet = MockDbSet(new List<UserTask> { userTask });
            _mockContext.Setup(c => c.UserTasks).Returns(mockSet);
            _taskRepository.DeleteTask(userId, Id);
            Assert.IsTrue(userTask.Flag);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void DeleteTasks_Sets_Flag_To_True_For_All_Tasks_And_Saves_Changes()
        {
            int userId = 1;
            var userTasks = new List<UserTask>
            {
                new UserTask { UserId = userId, Id = 1, Flag = false },
                new UserTask { UserId = userId, Id = 2, Flag = false },
                new UserTask { UserId = userId, Id = 3, Flag = false }
            }.AsQueryable();
            _mockContext.Setup(c => c.UserTasks).Returns(MockDbSet(userTasks));
            _taskRepository.DeleteTasks(userId);
            foreach (var task in userTasks)
            {
                Assert.IsTrue(task.Flag);
            }
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void ChangeTaskStatus_Toggles_Between_Active_And_Completed()
        {
            int userId = 1;
            int id = 1;
            var initialStatus = TaskState.Active;
            var userTasks = new List<UserTask>
            {
                new UserTask { UserId = userId, Id = id, StatusId = (int)initialStatus }
            };
            _mockContext.Setup(c => c.UserTasks).Returns(MockDbSet(userTasks));
            _taskRepository.ChangeTaskStatus(userId, id);
            var updatedUserTask = userTasks.First();
            if (initialStatus == TaskState.Active)
            {
                Assert.AreEqual((int)TaskState.Completed, updatedUserTask.StatusId);
                Assert.IsNotNull(updatedUserTask.CompletedOn);
            }
            else
            {
                Assert.AreEqual((int)TaskState.Active, updatedUserTask.StatusId);
                Assert.IsNull(updatedUserTask.CompletedOn);
            }
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void UpdateTask_WhenTaskIdIsZero_ShouldAddNewTaskInfoAndUpdateUserTask()
        {
            var taskInfos = new List<TaskInfo>();
            var userTasks = new List<UserTask>();
            _mockContext.Setup(c => c.TaskInfos).Returns(MockDbSet(taskInfos));
            _mockContext.Setup(c => c.UserTasks).Returns(MockDbSet(userTasks));
            var taskRequestDTO = new TaskRequestDTO
            {
                Title = "Sample Task",
                Description = "Task Description"
            };
            int userId = 1;
            int taskId = 0;
            int taskIdAfterSave = 1;
            int taskIdToUpdate = 1;
            int userTaskIdToUpdate = 1;
            _mockContext.Setup(c => c.SaveChanges()).Returns(1);
            var existingUserTask = new UserTask
            {
                Id = userTaskIdToUpdate,
                UserId = userId,
                TaskId = taskIdToUpdate
            };
            userTasks.Add(existingUserTask);
            _mockContext.Setup(c => c.TaskInfos.Add(It.IsAny<TaskInfo>())).Callback<TaskInfo>(task =>
            {
                taskInfos.Add(task);
                taskInfos.Last().Id = taskIdAfterSave;
            });
            _mockContext.Setup(c => c.UserTasks.Add(It.IsAny<UserTask>())).Callback<UserTask>(userTask =>
            {
                var existing = userTasks.FirstOrDefault(x => x.Id == userTask.Id);
                if (existing != null)
                {
                    existing.TaskId = userTask.TaskId;
                }
                else
                {
                    userTasks.Add(userTask);
                }
            });
            _taskRepository.UpdateTask(taskRequestDTO, userId, userTaskIdToUpdate);
            Assert.AreEqual(1, taskInfos.Count);
            Assert.AreEqual(1, userTasks.Count);
            var updatedUserTask = userTasks.First();
            Assert.AreEqual(taskIdAfterSave, updatedUserTask.TaskId);
            _mockContext.Verify(c => c.SaveChanges(), Times.Exactly(2));
        }

        [TestMethod]
        public void PerformanceIndicator_Returns_KpiDTO()
        {
            int userId = 1;
            var activeTasks = new List<UserTask>
            {
                new UserTask { Id = 1, UserId = userId, TaskId = 1, CreatedOn = DateTime.UtcNow, StatusId = 1, Flag = false, Task = new TaskInfo { Id = 1, Title = "Task 1", Description = "Description 1" }, User = new User { Id = userId }, Status = new Status { Id = 1 } }
            }.AsQueryable();
            var completedTasks = new List<UserTask>
            {
                new UserTask { Id = 1, UserId = userId, TaskId = 1, CreatedOn = DateTime.UtcNow, StatusId = 1, Flag = false, Task = new TaskInfo { Id = 1, Title = "Task 1", Description = "Description 1" }, User = new User { Id = userId }, Status = new Status { Id = 2 } }
            }.AsQueryable();
            _mockContext.Setup(c => c.UserTasks).Returns(MockDbSet(activeTasks.Concat(completedTasks)));
            var result = _taskRepository.PerformanceIndicator(userId);
            Assert.IsNotNull(result);
            Assert.AreEqual(50, result.ActiveTasks); 
            Assert.AreEqual(50, result.CompletedTasks);
        }

        private static DbSet<T> MockDbSet<T>(IEnumerable<T> data) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.AsQueryable().Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet.Object;
        }
    }
}