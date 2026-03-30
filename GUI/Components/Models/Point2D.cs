namespace GUI.Components.Models;

public class Point2D
{
    public int X { get; private set; }
    public int Y { get; private set; }
    
    public Point2D(){
        X = -1;
        Y = -1;
    }

    public Point2D(int X, int Y)
    {
         this.X = X;
         this.Y = Y;
    }
}