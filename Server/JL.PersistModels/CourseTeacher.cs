﻿using JL.PersistModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Persist
{
    [Table("CourseTeacher", Schema = "JL")]
    public class CourseTeacher : IPersist
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public bool OnLesson { get; set; }
    }
}
