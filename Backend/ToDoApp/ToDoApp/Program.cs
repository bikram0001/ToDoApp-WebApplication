using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ToDoApp.Middlewares;
using ToDoApp.Repository;
using ToDoApp.Repository.Interfaces;
using ToDoApp.Services.Interfaces;
using ToDoApp.Services.TaskManagement;
using ToDoApp.Services.UserManagement;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero,
    };
});

builder.Services.AddAuthorization();

builder.Services.AddTransient<ITaskContract, TaskServices>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddTransient<IUserContract, UserServices>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddDbContext<ToDoAppContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseMiddleware<UserContext>();
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
