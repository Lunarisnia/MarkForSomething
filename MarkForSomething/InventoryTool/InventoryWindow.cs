using ECommons;
using FFXIVClientStructs.FFXIV.Common.Math;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace MarkForSomething.InventoryTool;

public sealed class InventoryWindow
{
    // TODO: have this return the pos and be the inventory position main helper
    public static unsafe Vector2 GetInventoryPosition()
    {
        if (GenericHelpers.TryGetAddonByName<AtkUnitBase>("InventoryGrid0E", out var inventoryExpansion) && GenericHelpers.IsAddonReady(inventoryExpansion))
        {
            var inventoryGrid0E = inventoryExpansion->RootNode;
            Plugin.Log.Debug($"Inventory expansion found. {inventoryGrid0E->X} | {inventoryGrid0E->Y}");
            return new(0, 0);
        }
        return new(0, 0);
    }
}
