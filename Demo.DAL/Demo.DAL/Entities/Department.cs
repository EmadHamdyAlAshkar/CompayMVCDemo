﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Department : BaseEntity
    {
        public int Id { get; set; }
        [Required (ErrorMessage ="Department name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Department code is required")]

        public string Code { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
