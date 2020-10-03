using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stockr.Backend.Data.Objects
{
    public class User
    {
        public static List<User> Users = new List<Objects.User>();

        public static User Find(string username)
        {
            Objects.User[] u = Users.Where(x => x.userName == username).ToArray();
            if (u.Length == 0) return null;
            return u[0];
        }

        public string userName, hashPassword;

        public User(string uName, string password)
        {
            this.userName = uName;
            this.hashPassword = Security.Hashing.Hash(password);
        }
    }
}
