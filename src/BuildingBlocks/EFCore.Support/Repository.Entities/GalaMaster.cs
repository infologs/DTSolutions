﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Repository.Entities
{
    public class GalaMaster
    {
        public int Sr { get; }
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
