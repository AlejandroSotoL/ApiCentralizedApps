using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Repositories.Interfaces;
using Microsoft.AspNetCore.DataProtection;

namespace CentralizedApps.Repositories
{
    public class PasswordService : IPasswordService
    {
        private readonly IDataProtector _protector;

        public PasswordService(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("PasswordProtector");
        }

        public string Encrypt(string plainText)
        {
            return _protector.Protect(plainText);
        }

        public string Decrypt(string cipherText)
        {
            return _protector.Unprotect(cipherText);
        }
    }
}