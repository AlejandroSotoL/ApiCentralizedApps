using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralizedApps.Repositories.Interfaces
{
    public interface IPasswordService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
}