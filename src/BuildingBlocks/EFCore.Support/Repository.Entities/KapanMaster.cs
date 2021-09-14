﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class KapanMaster
    {
        public int Sr { get; }
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal CaratLimit { get; set; }
        public bool IsStatus { get; set; }
        public bool IsDelete { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
