namespace AirPad.Models;

public enum CommandType
{
    LeftClick,
    RightClick,
    MiddleClick,
    DoubleClick,
    ScrollUp,
    ScrollDown,
    VolumeUp,
    VolumeDown,
    Mute,
    ShowDesktop,
    TaskView
}

public class MacroCommand
{
    public CommandType Command { get; set; }
}