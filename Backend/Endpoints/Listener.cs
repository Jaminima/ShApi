using System;
using System.IO;
using System.Net;

namespace ShApi.Backend.Endpoints
{
    public static class Listener
    {
        #region Methods

        private static async void PreHandler(IAsyncResult result)
        {
            HttpListenerContext context = listener.EndGetContext(result);
            listener.BeginGetContext(PreHandler, null);

            if (context.Request.IsWebSocketRequest) await SocketHandler.HandleWebsocket(context);
            else
            {
                StreamReader stream = new StreamReader(context.Request.InputStream);
                string streamString = stream.ReadToEnd();

                Response response = new Response();

                RequestHandler.Handle(context.Request, streamString, ref response);

                response.Send(context.Response);
            }
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
}