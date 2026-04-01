using System.Net;
using System.Net.Sockets;
using GUI.Components.Controllers;

namespace GUI.Components.Pages;

public  partial class SnakeGUI{
   
    public string Name = "snake" ;
    public  int Port = 11000;
    public string ServerAddress = "localhost";
    NetworkController _controller = new NetworkController();

  public void Start(){ _controller.Connect(ServerAddress, Port, Name); }
  

  public void Stop() { _controller.Disconnect(); }
}