namespace GUI.Components.Models;

public class Player
{
    /// <summary>
    /// Snake ID
    /// </summary>
    public int  SnakeiD { get; set; }
    
    /// <summary>
    /// Client Name
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Snake Body Location
    /// </summary>
    public List<Point2D> Body { get; set; }
    
    /// <summary>
    /// Snake Direction
    /// </summary>
    public Point2D Dir { get; set; }
    
    /// <summary>
    /// Score
    /// </summary>
    public int Score { get; set; }
    
    /// <summary>
    /// Snake Dead
    /// </summary>
    public bool Dead { get; set; }
    
    /// <summary>
    /// Snake Alive
    /// </summary>
    public bool Alive { get; set; }
    
    /// <summary>
    /// Client Disconnected
    /// </summary>
    public bool Dc { get; set; }
    
    /// <summary>
    /// Client connected/joined
    /// </summary>
    public bool Join {get; set;}

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