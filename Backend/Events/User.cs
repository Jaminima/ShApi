using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Stockr.Backend.Events
{
    public static class User
    {
        public static string SignIn(NameValueCollection Headers)
        {
            string uname = Headers["username"], pword = Headers["password"];
            if (uname!=null && pword!=null)
            {
                if (Data.MemoryHandler.Users[0].userName==uname && Security.Hashing.Match(Data.MemoryHandler.Users[0].hashPassword, pword))
                {
                    return "Success";
                }
            }
            return null;
        }
    }
}
