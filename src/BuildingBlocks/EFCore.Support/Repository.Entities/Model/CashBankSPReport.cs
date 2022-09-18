﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Repository.Entities.Model
{
    public class CashBankSPReport
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string ToParty { get; set; }
        public string FromParty { get; set; }
        public int CrDrType { get; set; }

        [Column(TypeName = "decimal(18, 4)")]
        public decimal Amount { get; set; }
    }
}