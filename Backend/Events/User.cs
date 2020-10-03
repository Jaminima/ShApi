using Newtonsoft.Json.Linq;
using Stockr.Backend.Data.Objects;
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
        public static bool Logout(NameValueCollection Headers, ref Response response)
        {
            string token = Headers["authtoken"], uname = Headers["username"];
            if (token != null && uname != null)
            {
                if (LoginTokens.RemoveUser(uname, token))
                {
                    response.StatusCode = 200;
                }
                else
                {
                    response.StatusCode = 401;
                    response.AddToData("Error", "You are already signed out");
                }
            }
            else
            {
                response.StatusCode = 400; 
                response.AddToData("Error", "username & authtoken must be provided");
            }
            return false;
        }

        public static bool SignUp(NameValueCollection Headers, ref Response response)
        {
            string uname = Headers["username"], pword = Headers["password"];
            if (uname != null && pword != null)
            {
                if (Data.Objects.User.Find(uname) == null)
                {
                    Data.Objects.User user = new Data.Objects.User(uname, pword);
                    Data.Objects.User.Users.Add(user);

                    response.AddToData("authtoken", LoginTokens.CreateToken(user));
                    response.StatusCode = 200;
                }
                else
                {
                    response.AddToData("Error", "User already exists");
                    response.StatusCode = 401;
                }
            }
            else 
            { 
                response.StatusCode = 400;
                response.AddToData("Error", "username & password must be provided");
            }
            return false;
        }

        public static bool SignIn(NameValueCollection Headers, ref Response response)
        {
            string uname = Headers["username"], pword = Headers["password"];
            if (uname != null && pword != null)
            {
                Data.Objects.User user = Data.Objects.User.Find(uname);
                if (user != null && Hashing.Match(user.hashPassword, pword))
                {
                    response.AddToData("authtoken",LoginTokens.CreateToken(user));
                    response.StatusCode = 200;
                    return true;
                }
                else
                {
                    response.AddToData("Error", "User doesnt exist or password is wrong");
                    response.StatusCode = 401;
                }
            }
            else 
            { 
                response.StatusCode = 400; 
                response.AddToData("Error", "username & password must be provided"); 
            }
            return false;
        }
    }
}
