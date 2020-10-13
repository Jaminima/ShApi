using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;

namespace ShApi.Backend.Endpoints
{
    public static class Listener
    {
        #region Methods

        private static void PreHandler(IAsyncResult result)
        {
            HttpListenerContext context = listener.EndGetContext(result);
            listener.BeginGetContext(PreHandler, null);

            StreamReader stream = new StreamReader(context.Request.InputStream);
            string streamString = stream.ReadToEnd();

            Response response = new Response();

            RequestHandler.Handle(context.Request, streamString, ref response);

            response.Send(context.Response);
        }

        #endregion Methods

        #region Fields

        public static HttpListener listener;

        #endregion Fields

        public static void Start(int port = 1234)
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:" + port + "/");

            listener.Start();
            listener.BeginGetContext(PreHandler, null);
        }
    }

    public class Response
    {
        #region Fields

        public JObject Data = JObject.Parse("{'Time':" + DateTime.Now.Ticks + "}");
        public int StatusCode = 500;

        #endregion Fields

        #region Methods

        public void AddObjectToData(string Header, object obj)
        {
            Data.Property("Time").AddAfterSelf(new JProperty(Header, JToken.FromObject(obj).ToString()));
        }

        public void AddToData(string Header, object stringable)
        {
            Data.Property("Time").AddAfterSelf(new JProperty(Header, stringable.ToString()));
        }

        public void Send(HttpListenerResponse response)
        {
            response.StatusCode = StatusCode;

            response.Headers.Add("Access-Control-Allow-Origin", "*");

            StreamWriter stream = new StreamWriter(response.OutputStream);
            if (Data != null) stream.Write(JToken.FromObject(Data).ToString());

            stream.Flush();
            stream.Close();
        }

        #endregion Methods
    }
}