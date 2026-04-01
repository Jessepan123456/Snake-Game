using System.Net;
using System.Net.Sockets;
using GUI.Components.Controllers;

namespace GUI.Components.Pages;

public  partial class SnakeGUI
{
    private string _name = "snake" ;
    private  int _port = 11000;
    private string _serverAddress = "localhost";
    NetworkController _controller = new NetworkController();

    /// <summary>
    /// Connect to Server
    /// </summary>
    private void ConnectToServer() { _controller.Connect(_serverAddress, _port, _name); }

    /// <summary>
    /// Disconnect from Server
    /// </summary>
    private void DisconnectFromServer() { _controller.Disconnect(); }

    /// <summary>
    /// Check if it connected to Server
    /// </summary>
    /// <returns></returns>
    private bool IsConnectedToServer() { return _controller.IsConnected(); }

    /// <summary>
    /// Disable Input when you disconnect
    /// </summary>
    /// <returns></returns>
    private bool DisableInput()
    {
        return !IsConnectedToServer();
    }
}