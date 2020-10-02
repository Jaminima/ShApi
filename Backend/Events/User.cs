using Newtonsoft.Json.Linq;
using Stockr.Backend.Endpoints;
using Stockr.Backend.Security;
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
        public static bool SignIn(NameValueCollection Headers, ref Response response)
        {
            string uname = Headers["username"], pword = Headers["password"];
            if (uname != null && pword != null)
            {
                Data.Objects.User user = Data.MemoryHandler.FindUser(uname);
                if (user != null && Hashing.Match(user.hashPassword, pword))
                {
                    response.AddToData("AuthToken",LoginTokens.CreateToken(user));
                    response.StatusCode = 200;
                    return true;
                }
                else
                {
                    response.AddToData("Error", "User doesnt exist or password is wrong");
                    response.StatusCode = 401;
                }
            }
            else { response.StatusCode = 400; response.AddToData("Error", "username & password must be provided"); }
            return false;
        }
    }
}
