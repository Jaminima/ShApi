using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Reflection;

namespace ShApi.Backend.Endpoints
{
    public static class RequestHandler
    {
        #region Fields

        private static MethodInfo[] methodInfos = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .SelectMany(x => x.GetMethods())
            .Where(x => x.GetCustomAttributes(typeof(Events.WebEvent), false).FirstOrDefault() != null).ToArray();

        #endregion Fields

        #region Methods

        public static void Handle(HttpListenerRequest request, string Data, ref Response response)
        {
            string url = request.RawUrl.ToLower(), method = request.HttpMethod.ToLower();

            MethodInfo[] tMethod = methodInfos.Where(x => x.GetCustomAttribute<Events.WebEvent>().Equals(url, method)).ToArray();

            if (tMethod.Length > 0) tMethod[0].Invoke(null, new object[] { request.Headers, response });
            else
            {
                response.StatusCode = 404;
                response.AddToData("error", "Page not found");
            }
        }

        public static async Task HandleWebsocket(HttpListenerContext context) 
        {
            WebSocketContext webSocketContext = null;
            try
            {
                webSocketContext = await context.AcceptWebSocketAsync(subProtocol: null);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                context.Response.Close();
                Console.WriteLine("Exception: {0}", e);
                return;
            }

            WebSocket webSocket = webSocketContext.WebSocket;

            try
            {
                byte[] receiveBuffer = new byte[1024];

                while (webSocket.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                    if (receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    }
                    else if (receiveResult.MessageType == WebSocketMessageType.Binary)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "Cannot accept binary frame", CancellationToken.None);
                    }
                    else
                    {
                        await webSocket.SendAsync(new ArraySegment<byte>(receiveBuffer, 0, receiveResult.Count), WebSocketMessageType.Text, receiveResult.EndOfMessage, CancellationToken.None);
                    }
                }
            }
            catch (Exception e) { Console.WriteLine("Exception: {0}", e); }
            finally
            {
                if (webSocket != null) webSocket.Dispose();
            }
        }
    }

        #endregion Methods
    }