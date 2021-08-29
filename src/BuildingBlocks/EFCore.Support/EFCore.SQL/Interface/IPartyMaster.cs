﻿using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.SQL.Interface
{
    public interface IPartyMaster
    {
        Task<List<PartyMaster>> GetAllPartyAsync();
        Task<PartyMaster> AddPartyAsync(PartyMaster partyMaster);
        Task<PartyMaster> UpdatePartyAsync(PartyMaster partyMaster);
        Task<bool> DeletePartyAsync(int partyId, bool isPermanantDetele = false);
    }
}
