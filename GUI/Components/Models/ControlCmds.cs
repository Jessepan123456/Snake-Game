namespace GUI.Components.Models;

public class ControlCmds
{
    public string moving { get; private set; }
    
    public  ControlCmds(string moving)
    {
        this.moving = moving;
    }

    public ControlCmds()
    {
        this.moving = "none";
    }
}