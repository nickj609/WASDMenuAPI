// Included libraries
using CounterStrikeSharp.API.Core;

// Declare namespace
namespace WASDSharedAPI;

// Declare interface
public interface IWasdMenuOption
{
    public int Index { get; set; }
    public IWasdMenu? Parent { get; set; }
    public string? OptionDisplay { get; set; }
    public Action<CCSPlayerController, IWasdMenuOption>? OnChoose { get; set; }

}