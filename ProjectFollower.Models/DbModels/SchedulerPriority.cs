﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectFollower.Models.DbModels
{
    public class SchedulerPriority
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }
        public int Sorting { get; set; }

    }
}
