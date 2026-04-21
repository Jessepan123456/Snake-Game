using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace WebServerDemo;

using Networking;

public static class WebServer
{
    /// <summary>
    ///     SQL Connection Login
    /// </summary>
    private const string SqlConnection = "server=atr.eng.utah.edu;database=u1548814;uid=u1548814;password=bittermelon1";

    /// <summary>
    ///     Http attribute for Good Connection
    /// </summary>
    private const string HttpOkHeader = "HTTP/1.1 200 OK\r\n" +
                                        "Connection: close\r\n" +
                                        "Content-Type: text/html; " +
                                        "charset=UTF-8\r\n\r\n";

    /// <summary>
    ///     Http attribute for Bad Connection
    /// </summary>
    private const string HttpBadHeader =
        "HTTP/1.1 404 Not Found\r\nConnection: close\r\nContent-Type: text/html; charset=UTF-8\r\n\r\n";

    /// <summary>
    ///     Dictionary for storing PlayerDatabases 
    /// </summary>
    private static Dictionary<int, PlayerDataBase> _playerList = new Dictionary<int, PlayerDataBase>();

    /// <summary>
    ///     Dictionary for storing all Games DataBase
    /// </summary>
    private static Dictionary<int, (string?, string?)> _gamesList = new Dictionary<int, (string?, string?)>();

    /// <summary>
    ///     Start the Web Server based on the port
    /// </summary>
    static void Main()
    {
        Console.WriteLine("Starting web server...");
        Server.StartServer(HandleHttpConnection, 80);
        Console.Read();
    }

    /// <summary>
    ///     Handles incoming HTTP Connection.
    ///     Parses the request and routes them to the correct http
    /// </summary>
    /// <param name="client"></param>
    private static void HandleHttpConnection(NetworkConnection client)
    {
        Console.WriteLine("New client connected!");
        string incomingMessage = client.ReadLine();
        Console.WriteLine(incomingMessage);

        if (incomingMessage.Contains("GET / "))
        {
            client.Send(HttpOkHeader +
                        "<html>\n  <h3>Welcome to the Snake Games Database!</h3>\n  <a href=\"/games\">View Games</a>\n</html>");
        }
        else if (incomingMessage.Contains("GET /games "))
        {
            _gamesList.Clear();
            AllGames();
            StringBuilder table = new StringBuilder();
            foreach (int id in _gamesList.Keys)
            {
                table.Append(
                    "<html>\n  <table border=\"1\">\n    <thead>\n      <tr>\n        <td>ID</td><td>Start</td><td>End</td>\n      </tr>\n    </thead>\n    <tbody>\n      <tr>\n        <td><a href=\"/games?gid=" +
                    id + "\">" + id + "</a></td>\n        <td>" + _gamesList[id].Item1 + "</td>\n        <td>" +
                    _gamesList[id].Item2 + "</td>\n      </tr>\n  </tbody>\n  </table>\n</html>");
            }

            client.Send(HttpOkHeader + table);
        }
        else if (incomingMessage.Contains($"GET /games?gid="))
        {
            _playerList.Clear();
            string url = incomingMessage.Split(' ')[1];
            string gameId = url.Split("gid=")[1];
            int id = int.Parse(gameId);

            AllPlayer(id);
            StringBuilder table = new StringBuilder();
            table.Append(
                $"<html>\n  <h3>Stats for Game {id}</h3>\n  <table border=\"1\">\n    <thead>\n      <tr>\n        <td>Player ID</td><td>Player Name</td><td>Max Score</td><td>Enter Time</td><td>Leave Time</td>\n      </tr>\n    </thead>\n    <tbody>");

            foreach (int playerId in _playerList.Keys)
            {
                table.Append(
                    $"<tr>\n        <td>{playerId}</td><td>{_playerList[playerId].PlayerName}</td><td>{_playerList[playerId].MaxScore}</td><td>{_playerList[playerId].EnterTime}</td><td>{_playerList[playerId].EndTime}</td>\n      </tr>");
            }

            table.Append(" </tbody>\n  </table>\n</html>");
            client.Send(HttpOkHeader + table);
        }
        else
        {
            client.Send(HttpBadHeader + "This webpage doesn't exist!");
        }

        client.Disconnect();
    }

    /// <summary>
    ///     Helper method for getting all the games database from ours database
    /// </summary>
    private static void AllGames()
    {
        using (MySqlConnection conn = new MySqlConnection(SqlConnection))
        {
            try
            {
                conn.Open();

                MySqlCommand command = conn.CreateCommand();
                command.CommandText = "SELECT * FROM Games";

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader!.Read())
                    {
                        _gamesList.TryAdd(Convert.ToInt32(reader["ID"]),
                            (reader?["StartTime"].ToString(), reader?["EndTime"].ToString()));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    /// <summary>
    ///     Helper method for getting all the player from the database based on what gameId was selected
    /// </summary>
    /// <param name="gameId"></param>
    private static void AllPlayer(int gameId)
    {
        using (MySqlConnection conn = new MySqlConnection(SqlConnection))
        {
            try
            {
                conn.Open();

                MySqlCommand command = conn.CreateCommand();
                command.CommandText = "SELECT * FROM Players Where GameID='" + gameId + "'";
                Console.Write(command.CommandText);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PlayerDataBase player = new PlayerDataBase(
                            Convert.ToInt32(reader["ID"]),
                            reader["name"].ToString()!,
                            Convert.ToInt32(reader["MaxScore"]),
                            reader["EnterTime"].ToString()!,
                            reader["EndTime"].ToString()!
                        );
                        _playerList.TryAdd((Convert.ToInt32(reader["ID"])), player);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}