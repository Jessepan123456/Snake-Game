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
        if (IsConnected())
        {
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
        try //subbject to change might be a bad idea or there is a bbettwe way to handle disconnection
        {

            string Id = Recv();
            Player client = new Player();
            GameWorld.Player.Add(int.Parse(Id), client);

            string size = Recv();
            GameWorld.Size = int.Parse(size);

            while (IsConnected())
            {
                string mess = Recv();
                Console.WriteLine(mess);
                if (Regex.IsMatch(mess, PlayerMatch)) //deserialze player packet
                {
                    // Console.WriteLine("player");

                    Player? player = JsonSerializer.Deserialize<Player>(mess);
                    if (player != null)
                    {
                        if (player.Dc)
                        {
                            GameWorld.Player.Remove(player.SnakeiD);
                        }

                        if (GameWorld.Player.ContainsKey(player.SnakeiD))
                        {
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
                    // Console.WriteLine("power");

                    PowerUp? power = JsonSerializer.Deserialize<PowerUp>(mess);
                    if (power != null)
                    {
                        if (power.Died)
                        {
                            GameWorld.PowerUp.Remove(power.PowerType);
                        }

                        if (GameWorld.PowerUp.ContainsKey(power.PowerType))
                        {
                            GameWorld.PowerUp[power.PowerType] = power; //Update
                        }
                        else
                        {
                            GameWorld.PowerUp.Add(power.PowerType, power);
                        }
                    }
                }

                if (Regex.IsMatch(mess, WallMatch))
                {
                    // Console.WriteLine("wall");
                    Walls? wall = JsonSerializer.Deserialize<Walls>(mess);
                    if (wall != null)
                    {
                        if (GameWorld.Walls.ContainsKey(wall.WallType))
                        {
                            GameWorld.Walls[wall.WallType] = wall;
                        }
                        else
                        {
                            GameWorld.Walls.Add(wall.WallType, wall);
                        }
                    }
                }
            }
        }
        catch
        {
            return;
        }
    }

    public void sendControl(string key)
    {
        Control input = new Control();
        
        if (key == "w" || key =="ArrowUp")
        {
            input.Moving = "up";
        }
        if (key == "s" || key == "ArrowDown")
        {
           input.Moving = "down";
        }
        if (key == "a" || key == "ArrowLeft")
        {
            input.Moving = "left";
        }

        if (key == "d"  || key == "ArrowRight")
        {
            input.Moving = "right";
        }

        var cmd = JsonSerializer.Serialize(input);
        //Console.WriteLine(cmd);
        _connection.Send(cmd);
    }
}