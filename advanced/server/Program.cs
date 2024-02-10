using System;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    private static readonly HttpListener listener = new HttpListener();
    private static readonly List<WebSocket> clients = new List<WebSocket>();

    static async Task Main(string[] args)
    {
        listener.Prefixes.Add("http://localhost:8080/");
        listener.Start();
        Console.WriteLine("Server started.");

        // Accept clients
        Task.Run(AcceptClients);

        // Send datetime every 5 seconds to all clients
        Task.Run(() => BroadcastTimeEvery5Seconds());

        // Prevent the main thread from closing
        Console.ReadLine();
    }

    private static async Task AcceptClients()
    {
        while (true)
        {
            var httpContext = await listener.GetContextAsync();
            if (httpContext.Request.IsWebSocketRequest)
            {
                HandleClient(httpContext);
            }
            else
            {
                httpContext.Response.StatusCode = 400;
                httpContext.Response.Close();
            }
        }
    }

    private static async void HandleClient(HttpListenerContext context)
    {
        var webSocketContext = await context.AcceptWebSocketAsync(subProtocol: null);
        var webSocket = webSocketContext.WebSocket;
        clients.Add(webSocket);

        // Send initial message to client
        string clientInfo = $"Connected: {DateTime.Now}, IP: {context.Request.RemoteEndPoint}";
        await SendMessageAsync(webSocket, clientInfo);

        // Listen to messages from the client
        Task.Run(() => ListenToClientMessages(webSocket));
    }

    private static async Task ListenToClientMessages(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];

        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received: {receivedMessage}");

                // Echo the received message back to the client
                await SendMessageAsync(webSocket, $"We received your message: {receivedMessage}");
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                clients.Remove(webSocket);
                break;
            }
        }
    }

    private static async Task BroadcastTimeEvery5Seconds()
    {
        while (true)
        {
            await Task.Delay(5000); // Wait for 5 seconds
            string timeMessage = DateTime.Now.ToString();
            foreach (var client in clients.ToArray()) // ToArray to avoid collection modified exception
            {
                if (client.State == WebSocketState.Open)
                {
                    await SendMessageAsync(client, timeMessage);
                }
            }
        }
    }

    private static async Task SendMessageAsync(WebSocket webSocket, string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
    }
}
