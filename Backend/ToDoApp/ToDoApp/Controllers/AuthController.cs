using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Models;
using ToDoApp.Models.ViewModels;
using ToDoApp.Services.Interfaces;

namespace ToDoApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserContract _userService;
        public AuthController(IUserContract userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(User user)
        {
            try
            {
                IActionResult response = Unauthorized(null);
                var token = _userService.Login(user);
                return token != null ? Ok(token) : response;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost("adduser")]
        public IActionResult UserDetails(User user)
        {
            try
            {
                _userService.AddUser(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        public IActionResult IsUserNameExists(string userName) {
            try
            {
                return Ok(_userService.IsUserNameExists(userName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("tokens")]
        public IActionResult GenerateTokensFromRefreshToken([FromBody]string refreshToken)
        {
            try
            {
                return Ok(_userService.GenerateTokensFromRefreshToken(refreshToken));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}