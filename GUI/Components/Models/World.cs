namespace GUI.Components.Models;

public class World
{
    public Dictionary<int, Player> Player;
    
    public Dictionary<int, PowerUp> Powerup;
    
    public int Size {get; private set;}

    public World(int _size)
    {
        this.Size = _size;
        this.Player = new Dictionary<int, Player>();
        this.Powerup = new Dictionary<int, PowerUp>();
    }
    
    public World()
    {
        this.Size = 1;
        this.Player = new Dictionary<int, Player>();
        this.Powerup = new Dictionary<int, PowerUp>();
    }

    public World(World world)
    {
        Size = world.Size;
        Player = new (world.Player);
        Powerup = new (world.Powerup);
    }
}