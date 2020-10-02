using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stockr.Backend.Data.Objects
{
    public class User
    {
        public string userName, hashPassword;

        public User(string uName, string password)
        {
            this.userName = uName;
            this.hashPassword = Security.Hashing.Hash(password);
        }
    }
}
