﻿using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.SQL.Models
{
    public class LoginResponse
    {
        public UserMaster UserMaster { get; set; }
    }
}
