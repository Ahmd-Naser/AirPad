using AirPad.Models;
using AirPad.Services;
using Microsoft.AspNetCore.SignalR;

namespace AirPad.Hubs;

public class AirPadHub : Hub
{
    private readonly IMouseSimulator _mouseSimulator;

    public AirPadHub(IMouseSimulator mouseSimulator)
    {
        _mouseSimulator = mouseSimulator;
    }

    public async Task SendMovement(TouchPayload payload)
    {
        _mouseSimulator.ProcessMovement(payload);
        await Task.CompletedTask; 
    }

    public async Task SendCommand(MacroCommand command)
    {
        _mouseSimulator.ProcessCommand(command);
        await Task.CompletedTask;
    }
}