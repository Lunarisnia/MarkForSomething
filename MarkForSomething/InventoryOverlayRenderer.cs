using Dalamud.Bindings.ImGui;
using FFXIVClientStructs.FFXIV.Common.Math;

namespace MarkForSomething;

public sealed class InventoryOverlayRenderer
{
    private static readonly Vector2 GridSize = new Vector2(44, 46);

    public static void DrawGridMarker(ImDrawListPtr drawList, Vector2 gridPosition)
    {
        drawList.AddRect(
            gridPosition,
            gridPosition + GridSize,
            ImGui.ColorConvertFloat4ToU32(new(1.0f, 1.0f, 0.0f, 1.0f)));
    }
}
