using System.Net;
using System.Net.Sockets;
using GUI.Components.Controllers;
using GUI.Components.Models;
using Microsoft.AspNetCore.Components;

namespace GUI.Components.Pages;

/// <summary>
///     Code for the Snake GUI Blazor component
/// </summary>
public partial class SnakeGUI
{
    /// <summary>
    ///     Client Name
    /// </summary>
    private string _name = "snake";
    
    /// <summary>
    ///     Port Connection
    /// </summary>
    private int _port = 11000;
    
    /// <summary>
    ///     Server Address
    /// </summary>
    private string _serverAddress = "localhost";
    
    /// <summary>
    ///     Handles all the network connection
    /// </summary>
    NetworkController _controller = new NetworkController();

    /// <summary>
    ///     Connect to Server
    /// </summary>
    private void ConnectToServer()
    {
        _controller.Connect(_serverAddress, _port, _name);
    }

    /// <summary>
    ///     Disconnect from Server
    /// </summary>
    private void DisconnectFromServer()
    {
        _controller.Disconnect();
    }

    /// <summary>
    ///     Check if it connected to Server
    /// </summary>
    /// <returns>True if connected, false if not</returns>
    private bool IsConnectedToServer()
    {
        return _controller.IsConnected();
    }

    /// <summary>
    ///     Disable Input when you connect
    /// </summary>
    /// <returns>True if connected, otherwise false</returns>
    private bool IsDisableInput()
    {
        return IsConnectedToServer();
    }

    /// <summary>
    ///     Sends the key control that was pressed
    /// </summary>
    /// <param name="key"></param>
    private void SendKeyCmd(string key)
    {
        _controller.SendControl(key);
    }

    /// <summary>
    ///     Receive the Game World from controller
    /// </summary>
    /// <returns>"CopyOfWorld"</returns>
    private World ReceiveWorld()
    {
        return _controller.SendCopyOfWorld();
    }

    /// <summary>
    ///     Get the player ID of that client
    /// </summary>
    /// <returns>ID</returns>
    public int GetPlayerId()
    {
        return _controller.GetPlayerId();
    }
}