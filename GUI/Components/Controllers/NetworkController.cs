using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.RegularExpressions;
using GUI.Components.Models;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Networking;

namespace GUI.Components.Controllers;

public class NetworkController
{
    private NetworkConnection _connection = new();

    private World GameWorld = new World();
    //private Dictionary<int, Player> Players = new Dictionary<int, Player>();


    private String PlayerMatch = "snake";
    private String WallMatch = "wall";
    private String PowerUpMatch = "power";

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
        string Id = Recv();
        Player client = new Player();
        GameWorld.Player.Add(int.Parse(Id), client);

        string size = Recv();
        GameWorld.Size = int.Parse(size);
        
        Console.WriteLine(Id);
        Console.WriteLine(GameWorld.Size);
         
        while (IsConnected())
        {
            
            string mess = Recv();
            // Console.WriteLine(mess);
            // GameWorld.Player =  Players;
            if (Regex.IsMatch(mess, PlayerMatch))   //deserialze player packet
            {
                Console.WriteLine("player");
                
                Player? player = JsonSerializer.Deserialize<Player>(mess);
                if(player != null)
                {
                    if(GameWorld.Player.ContainsKey(player.SnakeiD)){
                        GameWorld.Player[player.SnakeiD] = player;
                    }
                    else
                    {
                        GameWorld.Player.Add(player.SnakeiD, player);
                    }
                }
                
            }

            if (Regex.IsMatch(mess, PowerUpMatch))
            {
              Console.WriteLine("power");
              PowerUp? power = JsonSerializer.Deserialize<PowerUp>(mess);
              if(power != null)
              {
                  if(GameWorld.PowerUp.ContainsKey(power.Power)){
                      GameWorld.PowerUp[power.Power] = power;
                  }
                  else
                  {
                      GameWorld.PowerUp.Add(power.Power, power);
                  }
              }
            }

            if (Regex.IsMatch(mess, WallMatch))
            { 
                Console.WriteLine("wall");
                Walls? wall = JsonSerializer.Deserialize<Walls>(mess);
                if(wall != null)
                {
                    if(GameWorld.Walls.ContainsKey(wall.Wall)){
                        GameWorld.Walls[wall.Wall] = wall;
                    }
                    else
                    {
                        GameWorld.Walls.Add(wall.Wall, wall);
                    }
                }
            }
        }
    }
 
}