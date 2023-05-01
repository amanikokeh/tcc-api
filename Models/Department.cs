﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC_API.Models
{
    internal class Department
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name{ get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }

        public virtual ICollection<Student> Students { get; set; }

    }
}
