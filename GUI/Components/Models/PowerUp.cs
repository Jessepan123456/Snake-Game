using System.Text.Json.Serialization;

namespace GUI.Components.Models;

/// <summary>
///     Represent PowerUp and it information
/// </summary>
public class PowerUp
{
    /// <summary>
    ///     Power Type
    /// </summary>
    [JsonPropertyName(("power"))]
    public int PowerType { get; set; }
    
    /// <summary>
    ///     Power Location
    /// </summary>
    [JsonPropertyName(("loc"))]
    public Point2D Location { get; set; }
    
    /// <summary>
    ///     Power Died or not
    /// </summary>
    [JsonPropertyName(("died"))]
    public bool Died { get; set; }
    
    /// <summary>
    ///     Default Constructor
    /// </summary>
    public PowerUp()
    {
        PowerType = -1;
        Location = new Point2D();
        Died = false;
    }

    /// <summary>
    ///     Constructor for PowerUp
    /// </summary>
    /// <param name="power"></param>
    /// <param name="location"></param>
    /// <param name="died"></param>
    public PowerUp(int power, Point2D location, bool died )
    {
        PowerType = power;
        Location = location;
        Died = died;
    }
}