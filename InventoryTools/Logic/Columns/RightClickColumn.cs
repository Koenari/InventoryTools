using System.Numerics;
using CriticalCommonLib.Crafting;
using CriticalCommonLib.Models;
using CriticalCommonLib.Sheets;
using ImGuiNET;
using InventoryTools.Extensions;
using InventoryTools.Logic.Columns.Abstract;

namespace InventoryTools.Logic.Columns
{
    public class RightClickColumn : TextColumn
    {
        public override string? CurrentValue(InventoryItem item)
        {
            return null;
        }

        public override string? CurrentValue(ItemEx item)
        {
            return null;
        }

        public override string? CurrentValue(SortingResult item)
        {
            return null;
        }

        public override string Name { get; set; } = "Right Click";
        public override float Width { get; set; } = 1.0f;

        public override string HelpText { get; set; } =
            "You shouldn't see this, but if you do it's the column that adds in the right click functionality.";
        public override string FilterText { get; set; } = "";
        public override bool HasFilter { get; set; } = false;
        public override ColumnFilterType FilterType { get; set; } = ColumnFilterType.Text;

        public override void Draw(FilterConfiguration configuration, InventoryItem item, int rowIndex)
        {
            Draw(configuration, item.Item, rowIndex);
        }

        public override void Draw(FilterConfiguration configuration, SortingResult item, int rowIndex)
        {
            Draw(configuration, item.InventoryItem.Item, rowIndex);
        }

        public override void Draw(FilterConfiguration configuration, CraftItem item, int rowIndex)
        {
            var hoveredRow = -1;
            ImGui.Selectable("", false, ImGuiSelectableFlags.SpanAllColumns, new Vector2(0, 32) * ImGui.GetIO().FontGlobalScale);
            if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled & ImGuiHoveredFlags.AllowWhenOverlapped & ImGuiHoveredFlags.AllowWhenBlockedByPopup & ImGuiHoveredFlags.AllowWhenBlockedByActiveItem & ImGuiHoveredFlags.AnyWindow)) {
                hoveredRow = rowIndex;
            }
            if (hoveredRow == rowIndex && ImGui.IsMouseReleased(ImGuiMouseButton.Right))
            {
                ImGui.OpenPopup("RightClick" + rowIndex);
            }

            if (ImGui.BeginPopup("RightClick" + rowIndex))
            {
                item.DrawRightClickPopup(configuration);
                ImGui.EndPopup();
            }
        }

        

        public override void Draw(FilterConfiguration configuration, ItemEx item, int rowIndex)
        {
            var hoveredRow = -1;
            ImGui.Selectable("", false, ImGuiSelectableFlags.SpanAllColumns, new Vector2(0, 32) * ImGui.GetIO().FontGlobalScale);
            if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled & ImGuiHoveredFlags.AllowWhenOverlapped & ImGuiHoveredFlags.AllowWhenBlockedByPopup & ImGuiHoveredFlags.AllowWhenBlockedByActiveItem & ImGuiHoveredFlags.AnyWindow)) {
                hoveredRow = rowIndex;
            }
            if (hoveredRow == rowIndex && ImGui.IsMouseReleased(ImGuiMouseButton.Right))
            {
                ImGui.OpenPopup("RightClick" + rowIndex);
            }

            if (ImGui.BeginPopup("RightClick" + rowIndex))
            {
                item.DrawRightClickPopup();
                ImGui.EndPopup();
            }
        }
    }
}