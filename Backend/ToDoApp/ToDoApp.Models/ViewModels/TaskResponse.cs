using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Models.ViewModels
{
    public class TaskResponse
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? CompletedOn { get; set; }
    }
}
