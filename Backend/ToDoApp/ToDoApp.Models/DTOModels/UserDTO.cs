﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApp.Models.DTOModels
{
    public class UserDTO
    {
        public int? Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
