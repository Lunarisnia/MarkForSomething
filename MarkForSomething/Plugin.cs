using Dalamud.Game.ClientState.Keys;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using ECommons;

namespace MarkForSomething;
// NOTE: for now always assume expanded inventory view
// TODO: Need to track the position of the inventory window
// TODO: Need to calculate the bounding box & position of each grid

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IFramework Framework { get; private set; } = null!;
    [PluginService] internal static IKeyState KeyState { get; private set; } = null!;
    [PluginService] internal static IGameInventory GameInventory { get; private set; } = null!;
    [PluginService] internal static IPluginLog Log { get; private set; } = null!;

    private const string CommandName = "/pinventory";
    private const VirtualKey InventoryKey = VirtualKey.F9;
    private readonly InventoryManager inventoryManager;
    private bool inventoryKeyWasDown;

    public Plugin()
    {
        ECommonsMain.Init(PluginInterface, this);
        inventoryManager = new InventoryManager();

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Lists the items in your inventory."
        });

        Framework.Update += OnFrameworkUpdate;
    }

    public void Dispose()
    {
        Framework.Update -= OnFrameworkUpdate;
        CommandManager.RemoveHandler(CommandName);
    }

    private void OnFrameworkUpdate(IFramework framework)
    {
        var keyIsDown = KeyState[InventoryKey];
        if (keyIsDown && !inventoryKeyWasDown)
            ListInventory();

        inventoryKeyWasDown = keyIsDown;
    }

    private void OnCommand(string command, string args)
    {
        ListInventory();
    }

    private void ListInventory()
    {
        var items = inventoryManager.GetInventory();
        Log.Information("Inventory contains {ItemCount} occupied slots:", items.Count);

        // foreach (var item in items)
        //     Log.Information("{Container} slot {Slot}: item {ItemId} x{Quantity}",
        //         item.Container, item.Slot, item.ItemId, item.Quantity);
    }
}
