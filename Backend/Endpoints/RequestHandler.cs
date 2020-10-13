using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;

namespace Stockr.Backend.Endpoints
{
    public static class RequestHandler
    {
        static MethodInfo[] methodInfos = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).SelectMany(x => x.GetMethods()).Where(x => x.GetCustomAttributes(typeof(Events.WebEvent), false).FirstOrDefault() != null).ToArray();

        public static void Handle(HttpListenerRequest request, string Data, ref Response response)
        {
            string url = request.RawUrl.ToLower(), method = request.HttpMethod.ToLower();

            MethodInfo[] tMethod = methodInfos.Where(x => x.GetCustomAttribute<Events.WebEvent>().Equals(url,method)).ToArray();

            if (tMethod.Length > 0) tMethod[0].Invoke(null, new object[] { request.Headers, response });
        }
    }
}
