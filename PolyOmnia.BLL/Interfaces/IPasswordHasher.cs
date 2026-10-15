using System;
using System.Collections.Generic;
using System.Text;

namespace PolyOmnia.BLL.Interfaces
{
    public interface IPasswordHasher
    {
        (string Hash, string Salt) HashPassword(string password);
        bool VerifyPassword(string password, string hash, string salt);
    }
}
