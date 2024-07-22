using System;
using System.Collections.Generic;

namespace ToDoApp.Repository.Models;

public partial class Status
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();
}
