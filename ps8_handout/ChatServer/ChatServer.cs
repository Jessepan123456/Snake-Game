// <copyright file="ChatServer.cs" company="UofU-CS3500">
// Copyright (c) 2025 UofU-CS3500. All rights reserved.
// </copyright>

using System.Linq.Expressions;
using Networking;

// ReSharper disable once CheckNamespace
namespace Chatting;

/// <summary>
///   A simple ChatServer that handles clients separately and replies with a static message.
/// </summary>
public abstract class ChatServer
{
    private static List<NetworkConnection> _connection = new List<NetworkConnection>();
    /// <summary>
    ///   The main program.
    /// </summary>
    private static void Main( string[] _ )
    {
        Server.StartServer( HandleConnect, 11_000 );
        while (true)
        {
            var message = Console.ReadLine();
            foreach (var connection in _connection)
            {
                connection.Send(message);
            }
        }
        Console.Read(); // don't stop the program. Jesse idk if this is need anymore?

    }

    /// <summary>
    ///   <pre>
    ///     When a new connection is established, enter a loop that receives from and
    ///     replies to a client.
    ///   </pre>
    /// </summary>
    ///
    private static void HandleConnect( NetworkConnection connection )
    {
        bool hasSend = false;
        string name = "";
        
        lock (_connection)
        {
            _connection.Add(connection);
        }
        
        // handle all messages until disconnect.
        try
        {
            while ( true )
            {
                
                if (hasSend == false)
                {
                    name = connection.ReadLine();
                    hasSend = true;
                    Console.WriteLine("Client:" + name);
                    connection.Send("Your name is " + name);
                    Broadcast("Server:" + "welcome " + name);
                }
                var message = connection.ReadLine();
                if (!(message == ""))
                {
                    Console.WriteLine(name + ": " + message);
                    Broadcast(name + ": " + message);
                }
              
            }
        }
        catch ( Exception )
        {
            lock (_connection)
            {
                _connection.Remove(connection);
            }
           connection.Dispose();
        }
    }
    public static void Broadcast( string message )
    {
            lock(_connection)
            {
                foreach (NetworkConnection connection in _connection)
                {
                    try { connection.Send(message); }
                    catch {}
                }   
            }
        }
    }

