using System.Net;
using System.Net.Sockets;
using Networking;

namespace GUI.Components.Controllers;

public class NetworkController
{ 
  NetworkConnection _connection = new NetworkConnection();

  public void Connect(string Host, int Port, string Name)
  {
      _connection.Connect(Host, Port);
      _connection.Send(Name);
      
  }

  public void Disconnect()
  {
      _connection.Disconnect();
  }
  
  
}