using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Stockr.Backend.Endpoints
{
    public static class RequestHandler
    {
        public static void Handle(HttpListenerRequest request, string Data, ref Response response)
        {
            switch (request.RawUrl.ToLower())
            {
                case "/signup" when request.HttpMethod == "POST":
                    Events.User.SignUp(request.Headers, ref response);
                    break;

                case "/login" when request.HttpMethod == "POST":
                    Events.User.SignIn(request.Headers,ref response);
                    break;

                default:
                    response.StatusCode = 404;
                    response.AddToData("Error","Page Not Found");
                    break;
            }
        }
    }
}
