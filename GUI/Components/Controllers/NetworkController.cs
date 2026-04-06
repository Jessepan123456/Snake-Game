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

    private World _gameWorld = new World();
    //private Dictionary<int, Player> Players = new Dictionary<int, Player>();


    private String _playerMatch = "snake";
    private String _wallMatch = "wall";
    private String _powerUpMatch = "power";

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
            string id = Recv();
            Player client = new Player();
            _gameWorld.Player.Add(int.Parse(id), client);

            string size = Recv();
            _gameWorld.Size = int.Parse(size);

            while (IsConnected())
            {
                string mess = Recv();
                Console.WriteLine(mess);
                if (Regex.IsMatch(mess, _playerMatch)) //deserialze player packet
                {
                    // Console.WriteLine("player");

                    Player? player = JsonSerializer.Deserialize<Player>(mess);
                    if (player != null)
                    {
                        if (player.Dc)
                        {
                            _gameWorld.Player.Remove(player.SnakeiD);
                        }

                            _gameWorld.Player[player.SnakeiD] = player;
                     
                    }
                }

                if (Regex.IsMatch(mess, _powerUpMatch))
                {
                    // Console.WriteLine("power");

                    PowerUp? power = JsonSerializer.Deserialize<PowerUp>(mess);
                    if (power != null)
                    {
                        if (power.Died)
                        {
                            _gameWorld.PowerUp.Remove(power.PowerType);
                        }
                        
                            _gameWorld.PowerUp[power.PowerType] = power; //Update
                    }
                }

                if (Regex.IsMatch(mess, _wallMatch))
                {
                    // Console.WriteLine("wall");
                    Walls? wall = JsonSerializer.Deserialize<Walls>(mess);
                    if (wall != null)
                    { 
                        _gameWorld.Walls[wall.WallType] = wall;
                    }
                }
            }
        }
        catch
        {
            return;
        }
    }

    public void SendControl(string key)
    {
        Control input = new Control();

        if (key == "w" || key == "ArrowUp")
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

        if (key == "d" || key == "ArrowRight")
        {
            input.Moving = "right";
        }

        var cmd = JsonSerializer.Serialize(input);
        //Console.WriteLine(cmd);
        _connection.Send(cmd);
    }

    public World SendCopyOfWorld()
    {
        return _gameWorld;
    }
}