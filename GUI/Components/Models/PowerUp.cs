namespace GUI.Components.Models;

public class PowerUp
{
    public int power { get; private set; }
    public List<Point2D> loc { get; private set; }
    public bool died { get; private set; }
    
    public PowerUp(int power, List<Point2D> loc)
    {
        this.power = power;
        this.loc = loc;
    }

    public PowerUp()
    {
        this.power = 0;
        this.loc = new List<Point2D>();
        this.died = false;
    }
}