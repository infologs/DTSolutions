﻿using EFCore.SQL.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSUnitTest.EFCore.SQL
{
    [TestClass]
    public class UserMasterUnitTest
    {
        private readonly UserMasterRepository _userMasterRepository;

        public UserMasterUnitTest()
        {
            _userMasterRepository = new UserMasterRepository();
        }

        [TestMethod]
        public void AddUserRecord()
        {
            string tempId = Guid.NewGuid().ToString();
            _ = _userMasterRepository.AddUserAsync(new Repository.Entities.UserMaster
            {
                Id = tempId,
                Name = "abedre",
                //BranchId = Guid.Parse("b63ad0da-bb6c-41a0-a8c2-7b05478e31ac"),
                AadharCardNo = "2346",
                Address = "Suat",
                Address2 = "Suat",
                UserName = "abedre",
                HomeNo = "123465",
                IsDelete = false,
                MobileNo = "132456",
                Password="abedre",
                ReferenceBy= "",
                UserType = 1,
                CreatedBy = tempId,
                CreatedDate = DateTime.Now,
                UpdatedBy = tempId,
                UpdatedDate = DateTime.Now
            }).Result;

            List<UserMaster> result = _userMasterRepository.GetAllUserAsync().Result;
            bool tempResult = false;

            foreach (var item in result)
            {
                if(item.Id == tempId)
                {
                    tempResult = true;
                    
                }
            }
            Assert.IsTrue(tempResult);
        }

        [TestMethod]
        public void LoginCheck()
        {
            var result = _userMasterRepository.Login("abedre", "abedre").Result;
        }
    }
}
