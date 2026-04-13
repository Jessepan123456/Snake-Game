using System.Text.Json.Serialization;

namespace GUI.Components.Models;

/// <summary>
///     Represent the Player and it information
/// </summary>
public class Player
{
    /// <summary>
    ///     Snake ID
    /// </summary>
    [JsonPropertyName(("snake"))]
    public int  SnakeiD { get; set; }
    
    /// <summary>
    ///     Client Name
    /// </summary>
    [JsonPropertyName(("name"))]
    public string Name { get; set; }
    
    /// <summary>
    ///     Snake Body Location
    /// </summary>
    [JsonPropertyName(("body"))]
    public List<Point2D> Body { get; set; }
    
    /// <summary>
    ///     Snake Direction
    /// </summary>
    [JsonPropertyName(("dir"))]
    public Point2D Dir { get; set; }
    
    /// <summary>
    ///     Score
    /// </summary>
    [JsonPropertyName(("score"))]
    public int Score { get; set; }
    
    /// <summary>
    ///     Snake Dead
    /// </summary>
    [JsonPropertyName(("dead"))]
    public bool Dead { get; set; }
    
    /// <summary>
    ///     Snake Alive
    /// </summary>
    [JsonPropertyName(("alive"))]
    public bool Alive { get; set; }
    
    /// <summary>
    ///     Client Disconnected
    /// </summary>
    [JsonPropertyName(("dc"))]
    public bool Dc { get; set; }
    
    /// <summary>
    ///     Client connected/joined
    /// </summary>
    [JsonPropertyName(("join"))]
    public bool Join {get; set;}

    [JsonIgnore]
    public DateTime StartTime { get; set; }
    [JsonIgnore]
    public DateTime EndTime { get; set; }
    [JsonIgnore]
    public int MaxScore { get; set; }
 
    /// <summary>
    ///     Default Constructor for Player
    /// </summary>
    public Player()
    {
        SnakeiD = 0;
        Name = "";
        Body = new List<Point2D>();
        Dir = new Point2D();
        Score = 0;
        Dead = false;
        Alive = true;
        Dc = false;
        Join = true;
    }
}