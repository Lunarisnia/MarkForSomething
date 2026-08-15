using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Game.Inventory;
using ECommons;
using FFXIVClientStructs.FFXIV.Component.GUI;
using MarkForSomething.InventoryTool;

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

    private static readonly int InventoryOffset = 2;
    private static readonly int InventorySize = 36;

    public unsafe Vector2 GetInventorySlotPosition(int slotIndex)
    {
        if (GenericHelpers.TryGetAddonByName<AtkUnitBase>("InventoryGrid0E", out var inventoryExpansion) && GenericHelpers.IsAddonReady(inventoryExpansion))
        {
            // Clamp the index into 3, 37 as it is the index of the inventory grid
            var grid = (AtkComponentNode*)inventoryExpansion->UldManager.NodeList[Math.Clamp(slotIndex + 2, InventoryOffset, InventorySize + InventoryOffset)];

            return new(grid->ScreenX, grid->ScreenY);
        }

        return new(0, 0);
    }

    public unsafe List<Vector2> GetInventorySlotPositions()
    {
        var slotPositions = new List<Vector2>();
        var inventories = new AtkUnitBase*[PlayerInventory.Length];

        for (var i = 0; i < inventories.Length; i++)
        {
            if (GenericHelpers.TryGetAddonByName($"InventoryGrid{i}E", out inventories[i]) &&
                !GenericHelpers.IsAddonReady(inventories[i]))
            {
                inventories[i] = null;
            }
        }

        foreach (var inventory in inventories)
        {
            if (inventory == null)
                continue;

            for (var slotIndex = InventoryOffset; slotIndex <= InventorySize; slotIndex++)
            {
                var grid = (AtkComponentNode*)inventory->UldManager.NodeList[Math.Clamp(slotIndex, InventoryOffset, InventorySize + InventoryOffset)];
                slotPositions.Add(new(grid->ScreenX, grid->ScreenY));
            }
        }

        return slotPositions;
    }
    
    
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

        InventoryWindow.GetInventoryPosition();
        return items;
    }

    public readonly record struct InventoryItem(
        GameInventoryType Container,
        uint Slot,
        uint ItemId,
        int Quantity);
}
