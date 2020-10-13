using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Stockr.Backend.Endpoints
{
    public class Response
    {
        public JObject Data = JObject.Parse("{'Time':"+DateTime.Now.Ticks+"}");
        public int StatusCode = 500;

        public void AddToData(string Header, object stringable)
        {
            Data.Property("Time").AddAfterSelf(new JProperty(Header, stringable.ToString()));
        }
        public void AddObjectToData(string Header, object obj)
        {
            Data.Property("Time").AddAfterSelf(new JProperty(Header, JToken.FromObject(obj).ToString()));
        }

        public void Send(HttpListenerResponse response)
        {
            response.StatusCode = StatusCode;

            response.Headers.Add("Access-Control-Allow-Origin","*");

            StreamWriter stream = new StreamWriter(response.OutputStream);
            if (Data != null) stream.Write(JToken.FromObject(Data).ToString());

            stream.Flush();
            stream.Close();
        }
    }

    public static class Listener
    {
        public static HttpListener listener;

        public static void Start(int port = 1234)
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:" + port + "/");

            listener.Start();
            listener.BeginGetContext(PreHandler, null);
        }

        static void PreHandler(IAsyncResult result)
        {
            HttpListenerContext context = listener.EndGetContext(result);
            listener.BeginGetContext(PreHandler, null);

            StreamReader stream = new StreamReader(context.Request.InputStream);
            string streamString = stream.ReadToEnd();

            Response response = new Response();

            RequestHandler.Handle(context.Request, streamString, ref response);

            response.Send(context.Response);
        }
    }
}
