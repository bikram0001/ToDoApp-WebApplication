using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ToDoApp.Models.ViewModels;
using ToDoApp.Services.Interfaces;

namespace ToDoApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskController : ControllerBase
    { 
        private readonly ITaskContract _taskService;
        public TaskController(ITaskContract taskService)
        {
            _taskService = taskService;
        }
        [HttpPost]
        public IActionResult AddTask(TaskRequest task)
        {
            try {
                int UserId = (int)HttpContext.Items["UserId"];
                if (UserId != 0)
                {
                    _taskService.AddTask(task, UserId);
                    return Ok();
                }
                return BadRequest("Authentication token is unavailable or not provided");
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
                
        }
        [HttpGet]
        public IActionResult GetAllTasks()
        {
            try {
                int UserId = (int)HttpContext.Items["UserId"];
                if (UserId != 0)
                {
                    return Ok(_taskService.GetAllTasks(UserId));
                }
                return BadRequest("Authentication token is unavailable or not provided");
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpGet("Active")]
        public IActionResult GetActiveTasks()
        {
            try {
                int UserId = (int)HttpContext.Items["UserId"];
                if (UserId != 0)
                {
                    return Ok(_taskService.GetActiveTasks(UserId));
                }
                return BadRequest("Authentication token is unavailable or not provided");
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpGet("Completed")]
        public IActionResult GetCompletedTasks()
        {
            try {
                int UserId = (int)HttpContext.Items["UserId"];
                if (UserId != 0)
                {
                    return Ok(_taskService.GetCompletedTasks(UserId));
                }
                return BadRequest("Authentication token is unavailable or not provided");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("Task")]
        public IActionResult DeleteTask(int id) {
            try {
                int UserId = (int)HttpContext.Items["UserId"];
                if (UserId != 0)
                {
                    _taskService.DeleteTask(UserId, id);
                    return Ok();
                }
                return BadRequest("Authentication token is unavailable or not provided");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete]
        public IActionResult DeleteTasks() {
            try
            {
                int UserId = (int)HttpContext.Items["UserId"];
                if (UserId != 0)
                {
                    _taskService.DeleteTasks(UserId);
                    return Ok();
                }
                return BadRequest("Authentication token is unavailable or not provided");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut]
        public IActionResult ChangeTaskStatus(int id) {
            try
            {
                int UserId = (int)HttpContext.Items["UserId"];
                if (UserId != 0)
                {
                    _taskService.ChangeTaskStatus(UserId, id);
                    return Ok();
                }
                return BadRequest("Authentication token is unavailable or not provided");
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("Update")]
        public IActionResult UpdateTask([FromBody]TaskRequest task, [FromQuery]int id) {
            try
            {
                int UserId = (int)HttpContext.Items["UserId"];
                if (UserId != 0)
                {
                    _taskService.UpdateTask(task, UserId, id);
                    return Ok();
                }
                return BadRequest("Authentication token is unavailable or not provided");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("kpi")]
        public IActionResult PerformanceIndicator()
        {
            try
            {
                int UserId = (int)HttpContext.Items["UserId"];
                if (UserId != 0)
                {
                    return Ok(_taskService.PerformanceIndicator(UserId));
                }
                return BadRequest("Authentication token is unavailable or not provided");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
    