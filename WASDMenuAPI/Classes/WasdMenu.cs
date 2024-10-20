﻿// Included libraries
using WASDSharedAPI;
using CounterStrikeSharp.API.Core;

// Declare namespace
namespace WASDMenuAPI.Classes;

// Define class
public class WasdMenu : IWasdMenu
{
    public string? Title { get; set; } = "";
    public LinkedList<IWasdMenuOption>? Options { get; set; } = new();
    public LinkedListNode<IWasdMenuOption>? Prev { get; set; } = null;
    public LinkedListNode<IWasdMenuOption> Add(string display, Action<CCSPlayerController, IWasdMenuOption> onChoice)
    {
        if (Options == null)
            Options = new();
        WasdMenuOption newOption = new WasdMenuOption
        {
            OptionDisplay = display,
            OnChoose = onChoice,
            Index = Options.Count,
            Parent = this
        };
        return Options.AddLast(newOption);
    }
}