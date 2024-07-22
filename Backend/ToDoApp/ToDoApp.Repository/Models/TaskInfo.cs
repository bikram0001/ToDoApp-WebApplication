using System;
using System.Collections.Generic;

namespace ToDoApp.Repository.Models;

public partial class TaskInfo
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();
}
