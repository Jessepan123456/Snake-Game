namespace GUI.Components.Models;

public class Player
{
    public int  SnakeID { get; private set; }
    public string Name { get; private set; }
    public List<Point2D> Body { get; private set; }
    public Point2D Dir { get; private set; }
    public int Score { get; private set; }
    public bool Dead { get; private set; }
    public bool Alive { get; private set; }
    public bool Dc { get; private set; }
    public bool Join {get; private set;}


    public Player()
    {
        SnakeID = 0;
        Name = "";
        Body = new List<Point2D>();
        Dir = new Point2D();
        Score = 0;
        Dead = false;
        Alive = true;
        Dc = false;
        Join = true;
    }


    public Player(int SnakeID, String Name, int Score , int X, int Y)
    {
        this.SnakeID = SnakeID;
        this.Name = Name;
        this.Score = Score;
        Dir = new Point2D(X, Y);

    }
}