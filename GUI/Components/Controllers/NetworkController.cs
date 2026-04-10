using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.RegularExpressions;
using GUI.Components.Models;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Networking;

namespace GUI.Components.Controllers;

/// <summary>
/// The NetworkController handles all communication between the client and server.
/// - Network Connection
/// - Sending player input
/// - Receiving data from the server and updating the GUI
/// </summary>
public class NetworkController
{
    private NetworkConnection _connection = new();

    private World _gameWorld = new World();
    private String _playerPattern = "snake";
    private String _wallPattern = "wall";
    private String _powerUpPattern = "power";
    private int _playerId = 0;
    private object locker = new object();

    /// <summary>
    ///     Connect to the Server
    ///     Starts a background thread that continuously listens to the server
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
    /// <returns>True if connected, false if not</returns>
    public bool IsConnected()
    {
        return _connection.IsConnected;
    }

    /// <summary>
    ///     Help receive data from the server
    /// </summary>
    /// <returns>ReadLine</returns>
    public string Recv()
    {
        return _connection.ReadLine();
    }

    /// <summary>
    ///     Get the ID from the server for the snakes
    /// </summary>
    /// <returns>ID</returns>
    public int GetPlayerId()
    {
        return _playerId;
    }

    /// <summary>
    ///     Continously receives messages from the server.
    ///     - Each Message are JSON String
    ///     - We identify them and updates the model
    /// </summary>
    private void NetworkLoop()
    {
        try
        {
            lock (locker)
            {
                string id = Recv();
                _playerId = int.Parse(id);
                Player client = new Player();
                _gameWorld.Player.Add(int.Parse(id), client);

                string size = Recv();
                _gameWorld.Size = int.Parse(size);
            }

            while (IsConnected())
            {
                string mess = Recv();
               
                if (Regex.IsMatch(mess, _playerPattern))
                {
                    Player? player = JsonSerializer.Deserialize<Player>(mess);
                    if (player != null)
                    {
                        lock (locker)
                        {
                            if (player.Dc)
                            {
                                _gameWorld.Player.Remove(player.SnakeiD);
                            }
                            else
                            {
                                _gameWorld.Player[player.SnakeiD] = player;
                            }
                        }
                    }
                }

                if (Regex.IsMatch(mess, _powerUpPattern))
                {
                    PowerUp? power = JsonSerializer.Deserialize<PowerUp>(mess);
                    if (power != null)
                    {
                        lock (locker)
                        {
                            if (power.Died)
                            {
                                _gameWorld.PowerUp.Remove(power.PowerType);
                            }
                            else
                            {
                                _gameWorld.PowerUp[power.PowerType] = power;
                            }
                        }
                    }
                }

                if (Regex.IsMatch(mess, _wallPattern))
                {
                    Walls? wall = JsonSerializer.Deserialize<Walls>(mess);
                    if (wall != null)
                    {
                        lock (locker)
                        {
                            _gameWorld.Walls[wall.WallType] = wall;
                        }   
                    }
                }
            }
        }
        catch
        {
            _connection.Disconnect();
        }
    }

    /// <summary>
    ///     Converts keyboard input into movement commands and sends them to the server
    ///     as JSON.
    /// </summary>
    /// <param name="key"></param>
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
        _connection.Send(cmd);
    }

    /// <summary>
    ///     Send a Copy of the World
    /// </summary>
    /// <returns>GameWorld</returns>
    public World SendCopyOfWorld()
    {
        lock (locker)
        {
            return _gameWorld;
        }
    }
}