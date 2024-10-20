// Included libraries
using WASDSharedAPI;
using WASDMenuAPI.Classes;
using CounterStrikeSharp.API.Core;

// Declare namespace
namespace WASDMenuAPI;

// Define class
public class WasdMenuOption : IWasdMenuOption
{
    public int Index { get; set; }
    public IWasdMenu? Parent { get; set; }
    public string? OptionDisplay { get; set; }
    public Action<CCSPlayerController, IWasdMenuOption>? OnChoose { get; set; }
}