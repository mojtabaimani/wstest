using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class WebSocketClient
{
    static async Task Main(string[] args)
    {
        using (ClientWebSocket client = new ClientWebSocket())
        {
            Uri serverUri = new Uri("ws://localhost:8080/");
            await client.ConnectAsync(serverUri, CancellationToken.None);
            Console.WriteLine("Connected to the server");

            // Listen for messages from the server
            var receiveTask = ReceiveMessages(client);

            // Read from the console and send messages to the server
            await SendMessages(client);

            // Wait for the receive task to complete
            await receiveTask;
        }
    }

    private static async Task ReceiveMessages(ClientWebSocket client)
    {
        byte[] buffer = new byte[1024 * 4];
        while (client.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result;
            do
            {
                result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Message from server: {message}");
            } while (!result.EndOfMessage);
        }
    }

    private static async Task SendMessages(ClientWebSocket client)
    {
        while (client.State == WebSocketState.Open)
        {
            string message = Console.ReadLine();
            if (!string.IsNullOrEmpty(message))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
