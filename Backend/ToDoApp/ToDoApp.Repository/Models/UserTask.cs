using System;
using System.Collections.Generic;

namespace ToDoApp.Repository.Models;

public partial class UserTask
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int TaskId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? CompletedOn { get; set; }

    public int? StatusId { get; set; }

    public bool? Flag { get; set; }

    public virtual Status? Status { get; set; }

    public virtual TaskInfo? Task { get; set; }

    public virtual User? User { get; set; }
}
