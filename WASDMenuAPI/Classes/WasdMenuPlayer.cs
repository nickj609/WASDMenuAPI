// Included libraries
using System.Text;
using WASDSharedAPI;
using WASDMenuAPI.Classes;
using System.Text.Json.Nodes;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Localization;

// Declare namespace
namespace WASDMenuAPI;

// Declare class
public class WasdMenuPlayer
{
    // Define properties
    public string CenterHtml = "";
    public int VisibleOptions = 5;
    public WasdMenu? MainMenu = null;
    public PlayerButtons Buttons { get; set; }
    public static IStringLocalizer? Localizer = null;
    public required CCSPlayerController player { get; set; }
    public LinkedListNode<IWasdMenuOption>? MenuStart = null;
    public LinkedListNode<IWasdMenuOption>? CurrentChoice = null;

    // Define methods
    public void OpenMainMenu(WasdMenu? menu)
    {
        if (menu == null)
        {
            MainMenu = null;
            CurrentChoice = null;
            CenterHtml = "";
            return;
        }
        MainMenu = menu;
        VisibleOptions = menu.Title != "" ? 4 : 5;
        CurrentChoice = MainMenu.Options?.First;
        MenuStart = CurrentChoice;
        UpdateCenterHtml();
    }

    public void OpenSubMenu(IWasdMenu? menu)
    {
        if (menu == null)
        {
            CurrentChoice = MainMenu?.Options?.First;
            MenuStart = CurrentChoice;
            UpdateCenterHtml();
            return;
        }

        VisibleOptions = menu.Title != "" ? 4 : 5;
        CurrentChoice = menu.Options?.First;
        MenuStart = CurrentChoice;
        UpdateCenterHtml();
    }
    public void GoBackToPrev(LinkedListNode<IWasdMenuOption>? menu)
    {
        if (menu == null)
        {
            CurrentChoice = MainMenu?.Options?.First;
            MenuStart = CurrentChoice;
            UpdateCenterHtml();
            return;
        }

        VisibleOptions = menu.Value.Parent?.Title != "" ? 4 : 5;
        CurrentChoice = menu;
        if (CurrentChoice.Value.Index >= 5 )
        {
            MenuStart = CurrentChoice;
            for (int i = 0; i < 4; i++)
            {
                MenuStart = MenuStart?.Previous;
            }
        }
        else
            MenuStart = CurrentChoice.List?.First;
        UpdateCenterHtml();
    }

    public void CloseSubMenu()
    {
        if(CurrentChoice?.Value.Parent?.Prev == null)
            return;
        GoBackToPrev(CurrentChoice?.Value.Parent.Prev);
    }

    public void CloseAllSubMenus()
    {
        OpenSubMenu(null);
    }
    
    public void Choose()
    {
        if (player != null)
        {
            CurrentChoice?.Value.OnChoose?.Invoke(player, CurrentChoice.Value);
        }
    }

    public void ScrollDown()
    {
        if(CurrentChoice == null || MainMenu == null)
            return;
        CurrentChoice = CurrentChoice.Next ?? CurrentChoice.List?.First;
        MenuStart = CurrentChoice!.Value.Index >= VisibleOptions ? MenuStart!.Next : CurrentChoice.List?.First;
        UpdateCenterHtml();
    }
    
    public void ScrollUp()
    {
        if(CurrentChoice == null || MainMenu == null)
            return;
        CurrentChoice = CurrentChoice.Previous ?? CurrentChoice.List?.Last;
        if (CurrentChoice == CurrentChoice?.List?.Last && CurrentChoice?.Value.Index >= VisibleOptions)
        {
            MenuStart = CurrentChoice;
            for (int i = 0; i < VisibleOptions-1; i++)
                MenuStart = MenuStart?.Previous;
        }
        else
            MenuStart = CurrentChoice!.Value.Index >= VisibleOptions ? MenuStart!.Previous : CurrentChoice.List?.First;
        UpdateCenterHtml();
    }

    private void UpdateCenterHtml()
    {
        if (CurrentChoice == null || MainMenu == null)
            return;

        StringBuilder builder = new StringBuilder();
        int i = 0;
        int n = 0;

        LinkedListNode<IWasdMenuOption>? option = MenuStart!;
<<<<<<< Updated upstream
=======
        if (option.Value.Parent?.Title != "")
        {
            builder.AppendLine($"{Localizer?["menu.title.prefix"]}{option?.Value?.Parent?.Title}</font> <font class='fontSize-s stratum-bold-italic'>{CurrentChoice?.Value?.Index+1}/{MainMenu?.Options?.Count-1}</font></font><br>");
        }
>>>>>>> Stashed changes

        while (i < VisibleOptions && option != null )
        {
            if (option.Value.Parent?.Title != "")
            {
                builder.AppendLine($"{Localizer?["menu.title.prefix"]}{option.Value.Parent?.Title}</u><font color='white'> Item: {n}/{MainMenu.Options?.Count}<br>");
            }
            
            if (option == CurrentChoice)
            {
                builder.AppendLine($"{Localizer?["menu.selection.left"]} <font class='fontSize-m' color='white'>{option.Value.OptionDisplay}</font> {Localizer?["menu.selection.right"]}<br>");
            }
            else
            {
                builder.AppendLine($"<font class='fontSize-m' color='white'>{option.Value.OptionDisplay}</font><br>");
            }

            option = option.Next;
            i++;
            n++;
        }

        if (option != null && Localizer != null && Localizer["menu.more.options.below"] != "") // more options string.
        { 
            builder.AppendLine($"{Localizer?["menu.more.options.below"]} <br>");
        }
        
        if(option != null && (option?.Value?.Index.Equals(MainMenu?.Options?.Last) ?? false))
        {
            builder.Append($"<br>");
        }

        builder.AppendLine($"{Localizer?["menu.bottom.text"]}");
        CenterHtml = builder.ToString();
    }
}