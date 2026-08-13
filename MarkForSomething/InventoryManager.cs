using System.Collections.Generic;
using Dalamud.Game.Inventory;
using Dalamud.Plugin.Services;

namespace MarkForSomething;

public class InventoryManager
{
    private static readonly GameInventoryType[] PlayerInventory =
    [
        GameInventoryType.Inventory1,
        GameInventoryType.Inventory2,
        GameInventoryType.Inventory3,
        GameInventoryType.Inventory4,
    ];

    public IReadOnlyList<InventoryItem> GetInventory()
    {
        var items = new List<InventoryItem>();

        foreach (var inventoryType in PlayerInventory)
        {
            foreach (var item in Plugin.GameInventory.GetInventoryItems(inventoryType))
            {
                if (item.IsEmpty)
                    continue;

                items.Add(new InventoryItem(
                    item.ContainerType,
                    item.InventorySlot,
                    item.ItemId,
                    item.Quantity));
            }
        }

        return items;
    }

    public readonly record struct InventoryItem(
        GameInventoryType Container,
        uint Slot,
        uint ItemId,
        int Quantity);
}
