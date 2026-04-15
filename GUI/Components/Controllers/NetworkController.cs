using System.Text.Json;
using System.Text.RegularExpressions;
using GUI.Components.Models;
using MySql.Data.MySqlClient;
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
    /// <summary>
    ///     Network Connection
    /// </summary>
    private NetworkConnection _connection = new();

    /// <summary>
    ///     Main Game World
    /// </summary>
    private World _gameWorld = new World();

    /// <summary>
    ///     Regex pattern for JSON Deserializing for Walls, Players, PowerUps
    /// </summary>
    private String _playerPattern = "snake";

    private String _wallPattern = "wall";
    private String _powerUpPattern = "power";

    /// <summary>
    ///     Player ID
    /// </summary>
    private int _playerId;

    /// <summary>
    ///     Lock for Locking
    /// </summary>
    private object _locker = new object();
    
    private const string SqlConnection =
        "server=atr.eng.utah.edu;" +
        "database=u1548814;" +
        "uid=u1548814;" +
        "password=bittermelon1;";

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
            
            new Thread(NetworkLoop).Start();
            _connection.Send(name);

            DateTime StartTime = DateTime.Now;
            AddGameRow(SqlConnection,"Games","StartTime","EndTime",  $"'{StartTime.ToString("yyyy-MM-dd HH:mm:ss")}'","NULL");
           Console.WriteLine( "INSERT INTO " + "table" + " (" + "columns" + ") VALUES ('" + "value" + "')");
        }
    }

    /// <summary>
    ///     Disconnect from Server
    /// </summary>
    public void Disconnect()
    {
        _gameWorld = new World();
        // DateTime EndTime =  DateTime.Now;
        // UpdateRow(SqlConnection, 
        //     "Game", 
        //     "EndTime", 
        //     EndTime.ToString("yyyy-MM-dd HH:mm:ss"),
        //     "ID", $"{player.SnakeiD}");
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
            lock (_locker)
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
                        lock (_locker)
                        {
                            if (player.Dc)
                            {
                                _gameWorld.Player.Remove(player.SnakeiD);
                                DateTime EndTime =  DateTime.Now;
                                UpdateRow(SqlConnection, 
                                    "Players", 
                                    "EndTime", 
                                    EndTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                    "ID", $"{player.SnakeiD}");
                            }
                            else
                            {
                                if (!_gameWorld.Player.ContainsKey(player.SnakeiD))
                                {
                                   // AddRow(SqlConnection, "Players", "ID", $"{player.SnakeiD}");
                                    DateTime StartTime = DateTime.Now;
                                    UpdateRow(SqlConnection,
                                        "Players",
                                        "StartTime",
                                        StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                        "ID",
                                        $"{player.SnakeiD}");
                                }
                                else
                                {
                                    
                                }
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
                        lock (_locker)
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
                        lock (_locker)
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
        World cloneWorld;
        lock (_gameWorld)
        {
            cloneWorld = new World(_gameWorld);
            return cloneWorld;
        }
    }
    private static int AddGameRow(string connection,string table ,string startTime,string endTime , string v1, string v2 )
    {
        int gameID;
        using (MySqlConnection conn = new MySqlConnection(connection))
        {
            conn.Open();
            MySqlCommand command = conn.CreateCommand();
            command.CommandText = $"INSERT INTO {table} ({startTime},{endTime}) VALUES ({v1}, {v2});";
            try
            {
                command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message); 
            }
            command.CommandText = "SELECT last_insert_id();";
            gameID = Convert.ToInt32(command.ExecuteScalar());
        }

        return gameID;
    }
    
    
    private static void UpdateRow(string connection,string table ,string col1,string updatedValue ,string col2 ,string whichValue)
    {
        using (MySqlConnection conn = new MySqlConnection(connection))
        {
            conn.Open();
            MySqlCommand command = conn.CreateCommand();
            command.CommandText =  "UPDATE " + table +" SET " + col1 + " = " + updatedValue + " WHERE " + col2 + " = " + whichValue;
            try
            {
                command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
               Console.WriteLine(e.Message); 
            }
        }
        }
    }
