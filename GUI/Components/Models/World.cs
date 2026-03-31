namespace GUI.Components.Models;

public class World
{
    /// <summary>
    /// Player Client
    /// </summary>
    public Dictionary<int, Player> Player;
    
    /// <summary>
    /// PowerUp
    /// </summary>
    public Dictionary<int, PowerUp> PowerUp;
    
    /// <summary>
    /// World Size
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
    }
}