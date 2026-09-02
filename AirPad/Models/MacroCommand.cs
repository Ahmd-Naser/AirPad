namespace AirPad.Models;

public enum CommandType
{
    LeftClick,
    RightClick,
    LeftMouseDown, 
    LeftMouseUp,
    MiddleClick,
    DoubleClick,
    ScrollUp,
    ScrollDown,
    VolumeUp,
    VolumeDown,
    Mute,
    ShowDesktop,
    TaskView,
    SwitchWindow
}

public class MacroCommand
{
    public CommandType Command { get; set; }
}