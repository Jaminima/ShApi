using ShApi.Backend.Data;
using ShApi.Backend.Endpoints;
using ShApi.Backend.Security;
using System.Collections.Specialized;
using System.Net.WebSockets;

namespace ShApi.Backend.Events
{
    public static class User
    {
        #region Methods

        [WebEvent("/websocket", "Echo", true)]
        public static void TestWebSocket(WebSocket webSocket, SocketEvent @event, SocketResponse response)
        {
            response.AddToData("Text", "AHHHH");
        }

        [WebEvent("/account", "DELETE")]
        public static bool DeleteAccount(NameValueCollection Headers, ref Response response)
        {
            string token = Headers["authtoken"], uname = Headers["username"];
            if (token != null && uname != null)
            {
                if (LoginTokens.IsLoggedIn(uname, token))
                {
                    Data.Objects.User user = LoginTokens.FindUserByName(uname);
                    MemoryHandler.Users.DeleteMany(x => x.userName == user.userName);
                    response.StatusCode = 200;
                }
                else
                {
                    response.StatusCode = 401;
                    response.AddToData("Error", "authtoken is not valid");
                }
            }
            else
            {
                response.StatusCode = 400;
                response.AddToData("Error", "username & authtoken must be provided");
            }
            return false;
        }

        [WebEvent("/logout", "POST")]
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

        [WebEvent("/login", "POST")]
        public static bool SignIn(NameValueCollection Headers, ref Response response)
        {
            string uname = Headers["username"], pword = Headers["password"];
            if (uname != null && pword != null)
            {
                Data.Objects.User user = Data.Objects.User.Find(uname);
                if (user != null && Hashing.Match(user.hashPassword, pword))
                {
                    response.AddCookie("authtoken", LoginTokens.CreateToken(user));
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

        [WebEvent("/signup", "POST")]
        public static bool SignUp(NameValueCollection Headers, ref Response response)
        {
            string uname = Headers["username"], pword = Headers["password"];
            if (uname != null && pword != null)
            {
                if (Data.Objects.User.Find(uname) == null)
                {
                    Data.Objects.User user = new Data.Objects.User(uname, pword);
                    MemoryHandler.Users.Insert(user);

                    response.AddCookie("authtoken", LoginTokens.CreateToken(user));
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

        #endregion Methods
    }
}