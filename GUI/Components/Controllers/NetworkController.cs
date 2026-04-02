using System.Net;
using System.Net.Sockets;
using GUI.Components.Models;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Networking;

namespace GUI.Components.Controllers;

public class NetworkController
{
    private NetworkConnection _connection = new();

    private World GameWorld = new World();
    private Dictionary<int, Player> Players = new Dictionary<int, Player>();
    

    /// <summary>
    ///     Connect to the Server
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    /// <param name="name"></param>
    public void Connect(string host, int port, string name)
    { 
        _connection.Connect(host, port); 
        if (IsConnected()){
            _connection.Send(name);
            new Thread(NetworkLoop).Start();
        }
    }

    /// <summary>
    ///     Disconnect from Server
    /// </summary>
    public void Disconnect()
    {
        _connection.Disconnect();
    }

    /// <summary>
    ///     Is it connected to the server
    /// </summary>
    /// <returns>IsConnected</returns>
    public bool IsConnected()
    {
        return _connection.IsConnected;
    }

    public string Recv()
    {
        return _connection.ReadLine();
    }

    public void Send(string msg)
    {
        _connection.Send(msg);
    }
    
    private void NetworkLoop()
    {
        int counter = 0;
        while (true)
        {
            string mess = Recv();
            counter++;
            if (counter == 1)
            {  
                Player client = new Player();
               Players.Add(int.Parse(mess), client);
               
            }

            if (counter == 2)
            {
                GameWorld.Size = int.Parse(mess);
                
            }
            
            GameWorld.Player =  Players;
            
        }
    }
 
}