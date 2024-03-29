﻿using Repository.Entities;
using Repository.Entities.Model;
using Repository.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFCore.SQL.Interface
{
    public interface IKapanMappingMaster
    {
        Task<KapanMappingMaster> GetKapanMappingDetailAsync(string purchaseId);
        Task<List<KapanMappingMaster>> GetKapanMappingMaster(string companyId, string branchId, string financialYearId);
        Task<List<KapanMappingReportModel>> GetKapanMappingReport(string companyId, string branchId, string financialYearId);
        Task<List<KapanMapping>> GetPendingKapanMapping(string companyId, string branchId, string financialYearId);
        Task<KapanMappingMaster> AddKapanMappingAsync(KapanMappingMaster kapanMappingMaster);
        Task<KapanMappingMaster> UpdateKapanMappingMasterAsync(KapanMappingMaster kapanMappingMaster);
        Task<int> GetMaxSrNo(string companyId, string financialYearId);
        Task<bool> DeleteKapanMappingAsync(string kapanMappingId);
    }
}
