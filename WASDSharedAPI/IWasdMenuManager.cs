// Included libraries
using CounterStrikeSharp.API.Core;

// Declare namespace
namespace WASDSharedAPI;

// Declare interface
public interface IWasdMenuManager
{
    // Opens the menu as a main menu
    public void OpenMainMenu(CCSPlayerController? player, IWasdMenu? menu);
    // Closes all menus
    public void CloseMenu(CCSPlayerController? player);
    // Closes current submenu and goes back to previous menu/submenu
    public void CloseSubMenu(CCSPlayerController? player);
    // Closes all submenus and goes back to the main menu
    public void CloseAllSubMenus(CCSPlayerController? player);
    // Opens menu as a submenu
    public void OpenSubMenu(CCSPlayerController? player, IWasdMenu? menu);
    // Creates a new menu object
    public IWasdMenu CreateMenu(string title = "");
}