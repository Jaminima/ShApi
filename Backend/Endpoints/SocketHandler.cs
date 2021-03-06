﻿using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShApi.Backend.Endpoints
{
    public class SocketEvent
    {
        public string Action = "nothing";
    }

    public class SocketResponse
    {
        public JObject Data = JObject.Parse("{'Time':" + DateTime.Now.Ticks + "}");

        public void AddObjectToData(string Header, object obj)
        {
            Data.Property("Time").AddAfterSelf(new JProperty(Header, JToken.FromObject(obj).ToString()));
        }

        public void AddToData(string Header, object stringable)
        {
            Data.Property("Time").AddAfterSelf(new JProperty(Header, stringable.ToString()));
        }

        public async Task Send(WebSocket webSocket)
        {
            byte[] bData = Encoding.UTF8.GetBytes(Data.ToString());
            await webSocket.SendAsync(new ArraySegment<byte>(bData, 0, bData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    public class SocketHandler
    {
        public static async Task ForwardSocRequest(string url, string Data, WebSocket webSocket)
        {
            SocketResponse response = new SocketResponse();
            try
            {
                JToken jData = JObject.Parse(Data);
                SocketEvent @event = jData.ToObject<SocketEvent>();

                MethodInfo[] tMethod = RequestHandler.methodInfos.Where(x => x.GetCustomAttribute<Events.WebEvent>().Equals(url, @event.Action, true)).ToArray();

                if (tMethod.Length > 0)
                {
                    tMethod[0].Invoke(null, new object[] { webSocket, @event, response });
                    await response.Send(webSocket);
                }
                else
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.EndpointUnavailable, "Request does not correspond to a known event", CancellationToken.None);
                }
            }
            catch (Exception e) { Console.WriteLine("Exception: {0}", e); }
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
                        string stringBuffer = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);

                        await ForwardSocRequest(context.Request.RawUrl.ToLower(), stringBuffer, webSocket);
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
}