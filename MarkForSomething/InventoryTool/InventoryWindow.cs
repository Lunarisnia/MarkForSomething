using ECommons;
using FFXIVClientStructs.FFXIV.Common.Math;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace MarkForSomething.InventoryTool;

public sealed class InventoryWindow
{
    // FIXME: Make this work for other type of inventory other than expanded
    public static unsafe Vector2 GetInventoryPosition()
    {
        if (GenericHelpers.TryGetAddonByName<AtkUnitBase>("InventoryGrid0E", out var inventoryExpansion) && GenericHelpers.IsAddonReady(inventoryExpansion))
        {
            var inventoryGrid0E = inventoryExpansion->RootNode;
            return new(inventoryGrid0E->X, inventoryGrid0E->Y);
        }
        return new(0, 0);
    }
}
