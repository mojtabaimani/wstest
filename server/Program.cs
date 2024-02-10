using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class WebSocketServer
{
    public static async Task Main(string[] args)
    {
        HttpListener httpListener = new HttpListener();
        httpListener.Prefixes.Add("http://localhost:8080/");
        httpListener.Start();
        Console.WriteLine("Listening...");

        while (true)
        {
            HttpListenerContext httpContext = await httpListener.GetContextAsync();
            if (httpContext.Request.IsWebSocketRequest)
            {
                HttpListenerWebSocketContext webSocketContext = null;
                try
                {
                    webSocketContext = await httpContext.AcceptWebSocketAsync(subProtocol: null);
                    WebSocket webSocket = webSocketContext.WebSocket;
                    Console.WriteLine("WebSocket connection established");

                    await SendMessages(webSocket);
                }
                catch (Exception e)
                {
                    httpContext.Response.StatusCode = 400;
                    Console.WriteLine($"Exception: {e.Message}");
                }
            }
            else
            {
                httpContext.Response.StatusCode = 400;
            }
        }
    }

    private static async Task SendMessages(WebSocket webSocket)
    {
        byte[] buffer = Encoding.UTF8.GetBytes("Hello World");
        await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        Console.WriteLine("Message sent");
        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
    }
}
