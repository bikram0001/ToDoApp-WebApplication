using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Caching.Memory;

namespace ToDoApp.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class UserContext
    {
        private readonly RequestDelegate _next;

        public UserContext(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var value = httpContext.Items["UserId"];
            int existingUserId = value!= null ? (int)value : 0;
            if (existingUserId == 0) {
                var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (token != null)
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                    var userId = jsonToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
                    httpContext.Items["UserId"] = int.Parse(userId);
                }
            }
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddleWareExtensions
    {
        public static IApplicationBuilder UseMiddleWare(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserContext>();
        }
    }
}
