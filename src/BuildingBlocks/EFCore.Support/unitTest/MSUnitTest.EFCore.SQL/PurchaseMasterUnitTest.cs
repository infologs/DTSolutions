﻿using EFCore.SQL.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSUnitTest.EFCore.SQL
{
    [TestClass]
    public class PurchaseMasterUnitTest
    {
        private readonly PurchaseMasterRepository _purchaseMasterRepository;

        public PurchaseMasterUnitTest()
        {
            _purchaseMasterRepository = new PurchaseMasterRepository();
        }

        [TestMethod]
        public void GetMaxSlipNo()
        {
            long maxSlipNo = _purchaseMasterRepository.GetMaxSlipNo(Guid.NewGuid(), Guid.NewGuid()).Result;
            Assert.IsTrue(maxSlipNo == 0);
        }

        [TestMethod]
        public void GetMaxSerialNo()
        {
            long maxSerialNo = _purchaseMasterRepository.GetMaxSrNo(Guid.NewGuid(), Guid.NewGuid()).Result;
            Assert.IsTrue(maxSerialNo == 0);
        }
    }
}
