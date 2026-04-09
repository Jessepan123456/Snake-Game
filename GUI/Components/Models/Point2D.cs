namespace GUI.Components.Models;

/// <summary>
///     Represent the Point2D X and Y
/// </summary>
public class Point2D
{
    /// <summary>
    ///     X Position
    /// </summary>
    public int X { get; set; }
    /// <summary>
    ///     Y Position
    /// </summary>
    public int Y { get; set; } 
    
    /// <summary>
    ///     Default Constructor
    /// </summary>
    public Point2D(){
        X = -1;
        Y = -1;
    }
    
    /// <summary>
    ///     Constructor for Point2D
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public Point2D(int x, int y)
    {
        X = x;
        Y = y;
    }
}