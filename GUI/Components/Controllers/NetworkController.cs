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
    private String _playerPattern = "snake";
    private String _wallPattern = "wall";
    private String _powerUpPattern = "power";
    private int _playerId = 0;

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

    /// <summary>
    ///     Help receive data from the server
    /// </summary>
    /// <returns></returns>
    public string Recv()
    {
        return _connection.ReadLine();
    }

    /// <summary>
    ///     Get the ID from the server for the snakes
    /// </summary>
    /// <returns></returns>
    public int GetPlayerId()
    {
        return _playerId;
    }

    private void NetworkLoop()
    {
        try
        {
            string id = Recv();
            _playerId = int.Parse(id);
            Player client = new Player();
            _gameWorld.Player.Add(int.Parse(id), client);

            string size = Recv();
            _gameWorld.Size = int.Parse(size);

            while (IsConnected())
            {
                string mess = Recv();
                if (Regex.IsMatch(mess, _playerPattern))
                {
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

                if (Regex.IsMatch(mess, _powerUpPattern))
                {
                    PowerUp? power = JsonSerializer.Deserialize<PowerUp>(mess);
                    if (power != null)
                    {
                        if (power.Died)
                        {
                            _gameWorld.PowerUp.Remove(power.PowerType);
                        }

                        _gameWorld.PowerUp[power.PowerType] = power;
                    }
                }

                if (Regex.IsMatch(mess, _wallPattern))
                {
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
        _connection.Send(cmd);
    }

    public World SendCopyOfWorld()
    {
        return _gameWorld;
    }
}