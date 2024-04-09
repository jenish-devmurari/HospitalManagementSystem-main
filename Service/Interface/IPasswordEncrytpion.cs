﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IPasswordEncryption
    {
        string HashPassword(string password);
        bool VerifyPassword(string enteredPassword, string storedHashedPassword);
    }
}
