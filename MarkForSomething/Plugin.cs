using System.Collections.Generic;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Game.ClientState.Keys;
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
    [PluginService] internal static IFramework Framework { get; private set; } = null!;
    [PluginService] internal static IKeyState KeyState { get; private set; } = null!;
    [PluginService] internal static IGameInventory GameInventory { get; private set; } = null!;
    [PluginService] internal static IPluginLog Log { get; private set; } = null!;

    private const VirtualKey InventoryKey = VirtualKey.F9;
    private readonly InventoryManager inventoryManager;
    private List<Vector2> inventorySlotPositions = [];
    private bool inventoryKeyWasDown;
    private bool inventoryIsVisible;

    public Plugin()
    {
        ECommonsMain.Init(PluginInterface, this);
        inventoryManager = new InventoryManager();

        Framework.Update += OnFrameworkUpdate;
        PluginInterface.UiBuilder.Draw += DrawOverlay;
    }

    public void Dispose()
    {
        Framework.Update -= OnFrameworkUpdate;
        PluginInterface.UiBuilder.Draw -= DrawOverlay;
    }

    private void OnFrameworkUpdate(IFramework framework)
    {
        inventorySlotPositions = inventoryManager.GetInventorySlotPositions();
        inventoryIsVisible = inventorySlotPositions.Count > 0;

        var keyIsDown = KeyState[InventoryKey];
        if (keyIsDown && !inventoryKeyWasDown)
            ListInventory();

        inventoryKeyWasDown = keyIsDown;
    }

    private void DrawOverlay()
    {
        if (!inventoryIsVisible)
            return;

        var drawList = ImGui.GetForegroundDrawList();
        foreach (var position in inventorySlotPositions)
            InventoryOverlayRenderer.DrawGridMarker(drawList, position);
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
