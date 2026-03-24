// <copyright file="Server.cs" company="UofU-CS3500">
// Copyright (c) 2025 UofU-CS3500. All rights reserved.
// </copyright>


using System.Net;
using System.Net.Sockets;

namespace Networking;

/// <summary>
///   Represents a server task that waits for connections on a given
///   port and calls the provided delegate when a connection is made.
/// </summary>
public static class Server
{
   
    /// <summary>
    ///   Wait on a TcpListener for new connections. Alert the main program
    ///   via a callback (delegate) mechanism.
    /// </summary>
    /// <param name="handleConnect">
    ///   Handler for what the user wants to do when a connection is made.
    ///   This should be run asynchronously via a new thread.
    /// </param>
    /// <param name="port"> The port (e.g., 11000) to listen on. </param>
    public static void StartServer( Action<NetworkConnection> handleConnect, int port )
    {
        TcpListener listener = new( IPAddress.Any, port);
        listener.Start();

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            
            var networkConnection = new Networking.NetworkConnection(client);
            new Thread(() => handleConnect(networkConnection)).Start();
        }
    }

    /// <summary>
    /// 
    /// </summary>a
    /// <param name="client"></param>
    public static void HandleConnect(TcpClient client)
    {
        NetworkConnection clientConnection = new(client);
        
        while (clientConnection.IsConnected)
        {
            Console.Write("HI");
            string msg = clientConnection.ReadLine();
            Console.WriteLine("Message: " + msg);
            clientConnection.Send("Thanks for that message");
        
        }
    }
    
}


