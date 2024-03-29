﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    public class BoilProcessMaster
    {
        public int Sr { get; set; }
        [Key]
        public string Id { get; set; }
        public int BoilNo { get; set; }
        public string PurchaseDetailsId { get; set; }
        public int JangadNo { get; set; }
        public string CompanyId { get; set; }
        public string BranchId { get; set; }
        public string EntryDate { get; set; }
        public string EntryTime { get; set; }
        public string FinancialYearId { get; set; }
        public int BoilType { get; set; } //Send, Receive
        public string KapanId { get; set; }
        public string ShapeId { get; set; }
        public string SizeId { get; set; }
        public string PurityId { get; set; }

        [Column(TypeName = "decimal(18, 4)")]
        public decimal Weight { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal LossWeight { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal RejectionWeight { get; set; }
        public string HandOverById { get; set; }
        public string HandOverToId { get; set; }
        public string StockId { get; set; }
        public string SlipNo { get; set; }
        public int BoilCategoy { get; set; } // 0 -> Received Carats, 1-> Loss Carets -> Rejection Carates
        public string Remarks { get; set; }
        public string TransferId { get; set; }
        public string TransferType { get; set; }
        public string TransferEntryId { get; set; }
        public double TransferCaratRate { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string AccountToAssortDetailsId { get; set; }
    }
}
