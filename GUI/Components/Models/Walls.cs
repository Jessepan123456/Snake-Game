using System.Text.Json.Serialization;

namespace GUI.Components.Models;

public class Walls
{
    /// <summary>
    /// Wall ID
    /// </summary>
    [JsonPropertyName(("wall"))]
    public int Wall{get; set;}
    
    /// <summary>
    /// Start point of the Wall
    /// </summary>
    [JsonPropertyName(("p1"))]
    public Point2D P1{get; set;}
    
    /// <summary>
    /// End point of the Wall
    /// </summary>
    [JsonPropertyName(("p2"))]
    public Point2D P2{get; set;}

    /// <summary>
    ///     Default Constructor for Walls
    /// </summary>
    public Walls()
    {
        Wall = -1;
        P1 = new Point2D( 0,0 );
        P2 = new Point2D( 0,0 ); 
    }
     
    /// <summary>
    ///     Constructor for Walls
    /// </summary>
    /// <param name="walls"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    public Walls(int wall, Point2D p1, Point2D p2)
    {
        Wall = wall;
        P1 = p1;
        P2 = p2;
    }
}