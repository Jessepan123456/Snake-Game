namespace GUI.Components.Models;

public class PowerUp
{
    /// <summary>
    /// Power Type
    /// </summary>
    public int Power { get; set; }
    
    /// <summary>
    /// Power Location
    /// </summary>
    public List<Point2D> Location { get; set; }
    
    /// <summary>
    /// Power Died or not
    /// </summary>
    public bool Died { get; set; }
    
    /// <summary>
    ///     Default Constructor
    /// </summary>
    public PowerUp()
    {
        Power = -1;
        Location = new List<Point2D>();
        Died = false;
    }

    /// <summary>
    ///     Constructor for PowerUp
    /// </summary>
    /// <param name="power"></param>
    /// <param name="location"></param>
    /// <param name="died"></param>
    public PowerUp(int power, List<Point2D> location, bool died )
    {
        Power = power;
        Location = location;
        Died = died;
    }
}