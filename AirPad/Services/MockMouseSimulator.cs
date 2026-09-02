using AirPad.Models;

namespace AirPad.Services;

public class MockMouseSimulator : IMouseSimulator
{
    public void ProcessMovement(TouchPayload payload)
    {
        Console.WriteLine($"[Movement] Fingers: {payload.FingerCount} | X: {payload.DeltaX} | Y: {payload.DeltaY}");
    }

    public void ProcessCommand(MacroCommand command)
    {
        Console.WriteLine($"[Command] Executing: {command.Command}");
    }
}
