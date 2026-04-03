using System.Text.Json.Serialization;

namespace GUI.Components.Models;

public class Control
{
    /// <summary>
    /// Player movement direction
    /// </summary>
    [JsonPropertyName(("moving"))]
    public string Moving { get; set; } = "none";

    /// <summary>
    ///     Default Constructor for Control
    /// </summary>
    public Control()
    {
    }

    /// <summary>
    ///     Constructor for setting the movement
    /// </summary>
    /// <param name="moving"></param>
    public Control(string moving)
    {
        Moving = moving;
    }
}