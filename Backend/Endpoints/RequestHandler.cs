using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace ShApi.Backend.Endpoints
{
    public class Response
    {
        #region Fields

        public JObject Data = JObject.Parse("{'Time':" + DateTime.Now.Ticks + "}");
        public int StatusCode = 500;
        private CookieCollection cookies = new CookieCollection();

        #endregion Fields

        #region Methods

        public void AddCookie(string name, string value)
        {
            cookies.Add(new Cookie(name, value));
        }

        public void AddObjectToData(string Header, object obj)
        {
            Data.Property("Time").AddAfterSelf(new JProperty(Header, JToken.FromObject(obj).ToString()));
        }

        public void AddToData(string Header, object stringable)
        {
            Data.Property("Time").AddAfterSelf(new JProperty(Header, stringable.ToString()));
        }

        public virtual void Send(HttpListenerResponse response)
        {
            response.StatusCode = StatusCode;

            response.Headers.Add("Access-Control-Allow-Origin", "*");

            response.ContentType = "application/json";
            response.Cookies = cookies;

            StreamWriter stream = new StreamWriter(response.OutputStream);
            if (Data != null) stream.Write(JToken.FromObject(Data).ToString());

            stream.Flush();
            stream.Close();
        }

        #endregion Methods
    }

    public static class RequestHandler
    {
        #region Fields

        public static MethodInfo[] methodInfos = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .SelectMany(x => x.GetMethods())
            .Where(x => x.GetCustomAttributes(typeof(Events.WebEvent), false).FirstOrDefault() != null).ToArray();

        #endregion Fields

        #region Methods

        public static void Handle(HttpListenerRequest request, string Data, ref Response response)
        {
            string url = request.RawUrl.ToLower(), method = request.HttpMethod.ToLower();

            MethodInfo[] tMethod = methodInfos.Where(x => x.GetCustomAttribute<Events.WebEvent>().Equals(url, method, false)).ToArray();

            if (tMethod.Length > 0) tMethod[0].Invoke(null, new object[] { request.Headers, response });
            else
            {
                response.StatusCode = 404;
                response.AddToData("error", "Page not found");
            }
        }
    }

    #endregion Methods
}