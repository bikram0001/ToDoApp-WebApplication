using System;
using System.Collections.Generic;

namespace ToDoApp.Repository.Models;

public partial class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();
}
