
# WebSocket Examples in C#

This repository contains two sets of examples demonstrating how to implement WebSocket communication in C#. The examples are divided into a simple server/client implementation and an advanced server/client implementation.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

- .NET 5.0 SDK or later

### Installing Dependencies

Before running any of the projects, you must restore the dependencies. Navigate to the project's directory and use the following command:

```bash
git clone https://github.com/mojtabaimani/wstest.git
cd wstest
dotnet restore
```

## Simple Server/Client

The simple server and client examples demonstrate the basic setup for WebSocket communication in C#.

### Simple Server

Located in `./server/`, this project shows how to create a basic WebSocket server that listens for connections and sends a "Hello World" message to the client.

Run the server using:

```bash
dotnet run --project server
```

### Simple Client

Located in `./client/`, this client connects to the WebSocket server and prints any message it receives to the console.

Run the client using:

```bash
dotnet run --project client
```

## Advanced Client/Server

The advanced examples demonstrate a more complex scenario where the server sends detailed information to the client upon connection, echoes received messages with additional text, and periodically sends updates to all connected clients.

### Advanced Server

Located in `./advanced/server/`, this server accepts multiple client connections, sends each client a message containing their connection info, echoes received messages with "your message received", and sends date and time updates every 5 seconds.

Run the server using:

```bash
dotnet run --project advanced/server
```

### Advanced Client

Located in `./advanced/client/`, this client allows users to send messages to the server through the console and receive messages from the server, including echoed messages and periodic updates.

Run the client using:

```bash
dotnet run --project advanced/client
```

## Contributing

Contributions are welcome! If you find any issues or have suggestions for improvements, please open an issue or submit a pull request.

## License

This project is licensed under the [MIT License](LICENSE).
