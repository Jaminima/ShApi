using Stockr.Backend.Data.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Stockr.Backend.Data
{
    public static class MemoryHandler
    {
        public static Objects.User[] Users = new Objects.User[] { new Objects.User("Jaminima","William") };

        public static Objects.User FindUser(string username)
        {
            Objects.User[] u = Users.Where(x => x.userName == username).ToArray();
            if (u.Length == 0) return null;
            return u[0];
        }
    }
}
