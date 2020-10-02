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
                case "/login":
                    response.Data = Events.User.SignIn(request.Headers);
                    if (response.Data != null) response.StatusCode = 200;
                    break;

                default:
                    response.StatusCode = 404;
                    response.Data = "Page Not Found";
                    break;
            }
        }
    }
}
