﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Repository.Entities.Model
{
    public class AccountToAssoftReceiveReportModel
    {
        public string Id { get; set; }
        public string Dept { get; set; }
        public int Sr { get; set; }
        public string CompanyId { get; set; }
        public string BranchId { get; set; }
        public string FinancialYearId { get; set; }
        public int JangadNo { get; set; }
        public string EntryDate { get; set; }
        public string HandOverById { get; set; }
        public string HandOverByName { get; set; }
        public string HandOverToId { get; set; }
        public string HandOverToName { get; set; }
        public string KapanId { get; set; }
        public string KapanName { get; set; }
        public string ShapeId { get; set; }
        public string ShapeName { get; set; }
        public string SizeId { get; set; }
        public string SizeName { get; set; }
        public string NumberId { get; set; }
        public string NumberName { get; set; }
        public string PurityId { get; set; }
        public string PurityName { get; set; }
        public string Remarks { get; set; }
        public string SlipNo { get; set; }       
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Weight { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
