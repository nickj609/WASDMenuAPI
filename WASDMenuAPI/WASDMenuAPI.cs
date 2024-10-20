// Included libraries
using WASDSharedAPI;
using WASDMenuAPI.Classes;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Core.Capabilities;
using CounterStrikeSharp.API.Core.Attributes.Registration;

// Declare namespace
namespace WASDMenuAPI;

// Define class
public class WASDMenuAPI : BasePlugin
{
    // Define plugin properties
    public override string ModuleName => "WASDMenuAPI";
    public override string ModuleVersion => "1.0.2";
    public override string ModuleAuthor => "Interesting";
    public static readonly Dictionary<int, WasdMenuPlayer> Players = new();
    public static PluginCapability<IWasdMenuManager> WasdMenuManagerCapability = new ("wasdmenu:manager");
    
    // Define on load behavior
    public override void Load(bool hotReload)
    {
        WasdMenuPlayer.Localizer = Localizer;
        var wasdMenuManager = new WasdManager();
        Capabilities.RegisterPluginCapability(WasdMenuManagerCapability, () => wasdMenuManager);

        // Register event handlers
        RegisterEventHandler<EventPlayerActivate>((@event, info) =>
        {
            if (@event.Userid != null)
                Players[@event.Userid.Slot] = new WasdMenuPlayer
                {
                    player = @event.Userid,
                    Buttons = 0
                };
            return HookResult.Continue;
        });
        RegisterEventHandler<EventPlayerDisconnect>((@event, info) =>
        {
            if (@event.Userid != null) Players.Remove(@event.Userid.Slot);
            return HookResult.Continue;
        });
        
        // Register listeners
        RegisterListener<Listeners.OnTick>(OnTick);
        
        // Hot reload behavior
        if(hotReload)
            foreach (var pl in Utilities.GetPlayers())
            {
               Players[pl.Slot] = new WasdMenuPlayer
               {
                   player = pl,
                   Buttons = pl.Buttons
               };
            }
    }

    // Define method for OnTick actions
    public void OnTick()
    {
        foreach (var player in Players.Values.Where(p => p.MainMenu != null))
        {
            if(player != null && player.player != null)
            {
                if ((player.Buttons & PlayerButtons.Forward) == 0 && (player.player?.Buttons & PlayerButtons.Forward) != 0)
                {
                    player.ScrollUp();
                }
                else if((player.Buttons & PlayerButtons.Back) == 0 && (player.player?.Buttons & PlayerButtons.Back) != 0)
                {
                    player.ScrollDown();
                }
                else if((player.Buttons & PlayerButtons.Moveright) == 0 &&(player.player?.Buttons & PlayerButtons.Moveright) != 0)
                {
                    player.Choose();
                } else if ((player.Buttons & PlayerButtons.Moveleft) == 0 && (player.player?.Buttons & PlayerButtons.Moveleft) != 0)
                {
                    player.CloseSubMenu();
                }
                if (player.player?.Buttons != null)
                {
                    if (((long)player.player.Buttons & 8589934592) == 8589934592)
                    { 
                        player.OpenMainMenu(null);
                    }

                    player.Buttons = player.player.Buttons;
                    if(player.CenterHtml != "")
                        Server.NextFrame(() =>
                        player.player.PrintToCenterHtml(player.CenterHtml)
                    );
                } 
            }
        }
    }
}
