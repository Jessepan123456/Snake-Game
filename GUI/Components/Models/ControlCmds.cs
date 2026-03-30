namespace GUI.Components.Models;

public class Control
{
    /// <summary>
    ///     Allows the variable Moving to get and set
    /// </summary>
    public string Moving { get; set; } = "none";
    
    /// <summary>
    ///     Default Constructor for Control
    /// </summary>
    public Control() { }
    
    /// <summary>
    ///     Constructor for setting the movement
    /// </summary>
    /// <param name="moving"></param>
    public Control(string moving)
    {
        this.Moving = moving;
    }
}