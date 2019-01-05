﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.Model
{

    public class StudentGroup : Entity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
    }
}
