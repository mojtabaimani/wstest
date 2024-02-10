using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class WebSocketClient
{
    static async Task Main(string[] args)
    {
        ClientWebSocket client = new ClientWebSocket();
        try
        {
            Uri serverUri = new Uri("ws://localhost:8080/");
            await client.ConnectAsync(serverUri, CancellationToken.None);
            Console.WriteLine("Connected to the server");

            await ReceiveMessages(client);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
        }
        finally
        {
            client?.Dispose();
            Console.WriteLine("WebSocket closed");
        }
    }

    private static async Task ReceiveMessages(ClientWebSocket client)
    {
        byte[] buffer = new byte[1024];
        WebSocketReceiveResult result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
        Console.WriteLine($"Message from server: {message}");
    }
}
