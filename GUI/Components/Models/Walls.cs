namespace GUI.Components.Models;

public class Walls
{
    public int WallID{get; private set;}
    public Point2D P1{get; private set;}
    public Point2D P2{get; private set;}

    public Walls()
    {
        WallID = 1;
        P1 = new Point2D(-575,-575);
        P2 = new Point2D(575,575); 
    }
     
    public Walls(int WallID, Point2D P1, Point2D P2)
    {
        this.WallID = WallID;
        this.P1 = P1;
        this.P2 = P2;
        
    }
}