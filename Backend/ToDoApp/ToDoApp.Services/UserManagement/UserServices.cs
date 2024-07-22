using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ToDoApp.Repository.Interfaces;
using ToDoApp.Services.Interfaces;
using ToDoApp.Models.ViewModels;
using ToDoApp.Models.DTOModels;

namespace ToDoApp.Services.UserManagement
{
    public class UserServices : IUserContract
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        public UserServices(IConfiguration config,IUserRepository roleRepository)
        {
            _config = config;
            _userRepository = roleRepository;
        }

        public void AddUser(User user)
        {
            UserDTO userDTO = DTOViewMapper.MapToUserDTO(user);
            userDTO.Password = HashPassword(user.Password);
            _userRepository.AddUser(userDTO);
        }

        public UserDTO IsUserExists(User user)
        {
            UserDTO userDTO = DTOViewMapper.MapToUserDTO(user);
            return _userRepository.IsUserExists(userDTO);
        }

        public bool IsUserNameExists(string userName) { 
            return _userRepository.IsUserNameExists(userName);
        }

        private UserDTO? AuthenticateUser(User user)
        {
            if (user.Password != null)
            {
                user.Password = HashPassword(user.Password);
            }
            UserDTO userDTO = DTOViewMapper.MapToUserDTO(user);
            UserDTO existingUser = _userRepository.IsUserExists(userDTO);
            return existingUser;
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public UserDTO getUserDetails(int userId) { 
            return _userRepository.getUserDetails(userId);
        }

        public object? GenerateTokensFromRefreshToken(string refreshToken) {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(refreshToken) as JwtSecurityToken;
            var userId = jsonToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            UserDTO userDTO = getUserDetails(int.Parse(userId));
            return GenerateTokens(userDTO);
        }

        private string GenerateToken(UserDTO user)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim("userId",user.Id.ToString()),
                new Claim("userName",user.UserName)
            };
            var token = new JwtSecurityToken(issuer: _config["Jwt:Issuer"], claims: claims, expires: DateTime.Now.AddHours(1), signingCredentials: credentials, audience: _config["Jwt:Audience"]);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken(int userId) {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim("userId",userId.ToString())
            };
            var refreshToken = new JwtSecurityToken(issuer: _config["Jwt:Issuer"], claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials, audience: _config["Jwt:Audience"]);
            return new JwtSecurityTokenHandler().WriteToken(refreshToken);
        }

        public object? Login(User user)
        {
            UserDTO? userDTO = AuthenticateUser(user);
            return GenerateTokens(userDTO);
        }
        private object? GenerateTokens(UserDTO userDTO) {
            if (userDTO != null)
            {
                int userId = userDTO.Id ?? 0;
                var accessToken = GenerateToken(userDTO);
                var refreshToken = GenerateRefreshToken(userId);
                return new { token = accessToken, refreshToken };
            }
            return null;
        }
    }
}
