using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace WebServerDemo;

using Networking;

public static class WebServer
{
    public const string SqlConnection = "server=atr.eng.utah.edu;database=u1548814;uid=u1548814;password=bittermelon1";

    private const string HttpOkHeader = "HTTP/1.1 200 OK\r\n" +
                                        "Connection: close\r\n" +
                                        "Content-Type: text/html; " +
                                        "charset=UTF-8\r\n\r\n";

    private const string HttpBadHeader =
        "HTTP/1.1 404 Not Found\r\nConnection: close\r\nContent-Type: text/html; charset=UTF-8\r\n\r\n";

    private static Dictionary<int, (string?, string?)> _gamesList = new Dictionary<int, (string?, string?)>();

    static void Main()
    {
        Console.WriteLine("Starting web server...");
        AllGames();
        Server.StartServer(HandleHttpConnection, 80);
        Console.Read();
    }


    private static void HandleHttpConnection(NetworkConnection client)
    {
        Console.WriteLine("New client connected!");
        string incomingMessage = client.ReadLine();
        Console.WriteLine(incomingMessage);

        if (incomingMessage.Contains("GET / "))
        {
            client.Send(HttpOkHeader +
                        "<html>\n  <h3>Welcome to the Snake Games Database!</h3>\n  <a href=\"/games\">View Games</a>\n</html>");

            // This is a great start, but we need to programatically construct the HTML.
            // You can do this by writing whatever c# code you want. For example, the database
            // This code needs to query the database, find out who all is playing / what games exist, etc
            /*
             * <t>
             * for loop:
             *   For each game: <tr>
             *      <td>reader["name"] </td> + " " + ...
             *
             */
        }
        else if (incomingMessage.Contains("GET /games "))
        {
            string begining = HttpOkHeader +
                              "<html>\\n  <table border=\\\"1\\\">\\n    <thead>\\n      <tr>\\n        <td>ID</td><td>Start</td><td>End</td>\\n      </tr>\\n    </thead>\\n    <tbody>\\n      <tr>\\n    ";
            string ending =
                "</tr>\n      ... (more table rows omitted for brevity) ...\n    </tbody>\n  </table>\n</html>";
            StringBuilder table = new StringBuilder();

            foreach (int id in _gamesList.Keys)
            {
                table.Append(
                    "<html>\n  <table border=\"1\">\n    <thead>\n      <tr>\n        <td>ID</td><td>Start</td><td>End</td>\n      </tr>\n    </thead>\n    <tbody>\n      <tr>\n        <td><a href=\"/games?gid=" +
                    id + "\">" + id + "</a></td>\n        <td>" + _gamesList[id].Item1 + "</td>\n        <td>" +
                    _gamesList[id].Item2 + "</td>\n      </tr>\n  </tbody>\n  </table>\n</html>");
            }

            client.Send(HttpOkHeader + table.ToString());
        }
        else if (incomingMessage.Contains("GET /games?gid=8 "))
        {
            client.Send(HttpOkHeader +
                        "<html>\n  <h3>Stats for Game 8</h3>\n  <table border=\"1\">\n    <thead>\n      <tr>\n        <td>Player ID</td><td>Player Name</td><td>Max Score</td><td>Enter Time</td><td>Leave Time</td>\n      </tr>\n    </thead>\n    <tbody>\n      <tr>\n        <td>12</td><td>Danny</td><td>1</td><td>11/23/2024 10:41:29 AM</td><td>11/23/2024 10:41:53 AM</td>\n      </tr>\n      ... (more table rows omitted for brevity) ...\n    </tbody>\n  </table>\n</html>");
        }
        else
        {
            client.Send(HttpBadHeader + "This webpage doesn't exist!");
        }

        client.Disconnect();
    }

    private static void AllGames()
    {
        // Connect to the DB
        using (MySqlConnection conn = new MySqlConnection(SqlConnection))
        {
            try
            {
                // Open a connection
                conn.Open();

                // Create a command
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = "SELECT * FROM Games";

                // Execute the command and cycle through the DataReader object
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //Console.WriteLine(reader["ID"] + " " + reader["StartTime"] + " " + reader["EndTime"]);
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
}