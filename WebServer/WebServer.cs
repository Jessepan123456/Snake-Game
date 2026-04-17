namespace WebServerDemo;

using Networking;

public static class WebServer
{
    private const string HttpOkHeader = "HTTP/1.1 200 OK\r\n" +
                                        "Connection: close\r\n" +
                                        "Content-Type: text/html; " +
                                        "charset=UTF-8\r\n\r\n";
    
    private const string HttpBadHeader = "HTTP/1.1 404 Not Found\r\nConnection: close\r\nContent-Type: text/html; charset=UTF-8\r\n\r\n";

    static void Main()
    {
        Console.WriteLine("Starting web server...");
        // Use my PS8 code.
        // StartServer takes a Delegate. Starts listening at port 80
        // Whenever someone connects to port 80, run delegate with that client
        Server.StartServer(HandleHttpConnection, 80);
        Console.Read(); // prevent main from returning
    }
    

    private static void HandleHttpConnection(NetworkConnection client)
    {
        Console.WriteLine("New client connected!");
        string incomingMessage = client.ReadLine();
        Console.WriteLine(incomingMessage);
        
        // What if I wanted to send different things based on what the client wanted?
        // Look at the incoming message to see what was requested.

        if (incomingMessage.Contains("GET / "))
        {
            // Serve the home page HTML
            client.Send(HttpOkHeader + "<html>\n  <h3>Welcome to the Snake Games Database!</h3>\n  <a href=\"/games\">View Games</a>\n</html>");
            
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
            client.Send(HttpOkHeader + "<html>\n  <table border=\"1\">\n    <thead>\n      <tr>\n        <td>ID</td><td>Start</td><td>End</td>\n      </tr>\n    </thead>\n    <tbody>\n      <tr>\n        <td><a href=\"/games?gid=8\">8</a></td>\n        <td>11/23/2024 10:38:52 AM</td>\n        <td>11/23/2024 10:39:52 AM</td>\n      </tr>\n      ... (more table rows omitted for brevity) ...\n    </tbody>\n  </table>\n</html>");
            // client.Send(HttpOkHeader + "<h1>CATS CATS CATS</h1>");
            // for (int i = 0; i < 1000; i++)
            // {
            //     client.Send("<h1>CATS CATS CATS</h1>");
            // }
        }
        else if (incomingMessage.Contains("GET /games?gid=8 "))
        {
            
        }
        else // error case
        {
            client.Send(HttpBadHeader + "This webpage doesn't exist!");
        }
        client.Disconnect();
    }
}