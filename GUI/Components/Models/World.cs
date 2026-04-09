namespace GUI.Components.Models;

/// <summary>
///     Represent the Game World when Created
/// </summary>
public class World
{
    /// <summary>
    ///     Player Client
    /// </summary>
    public Dictionary<int, Player> Player;
    
    /// <summary>
    ///     PowerUp
    /// </summary>
    public Dictionary<int, PowerUp> PowerUp;
    
    /// <summary>
    ///     Walls 
    /// </summary>
    public Dictionary<int, Walls> Walls;
    
    /// <summary>
    ///     World Size
    /// </summary>
    public int Size {get; set;}

    /// <summary>
    ///     Default Constructor
    /// </summary>
    public World ()
    {
        Size = 1;
        Player = new Dictionary<int, Player>();
        PowerUp = new Dictionary<int, PowerUp>();
        Walls = new Dictionary<int, Walls>();
    }
    
    /// <summary>
    ///     Constructor for World
    /// </summary>
    /// <param name="size"></param>
    public World (int size)
    {
        Size = size;
        Player = new Dictionary<int, Player>();
        PowerUp = new Dictionary<int, PowerUp>();
        Walls = new Dictionary<int, Walls>();
    }

    /// <summary>
    ///     Copy World Constructor
    /// </summary>
    /// <param name="world"></param>
    public World (World world)
    {
        Size = world.Size;
        Player = new Dictionary<int, Player> (world.Player);
        PowerUp = new Dictionary<int, PowerUp> (world.PowerUp);
        Walls = new Dictionary<int, Walls> (world.Walls);
    }
}