using CriticalCommonLib.Models;
using Dalamud.Interface.Colors;
using ImGuiNET;

namespace InventoryTools.Logic.Columns
{
    public class MarketBoardTotalPriceColumn : MarketBoardPriceColumn
    {
        public override FilterType AvailableIn => Logic.FilterType.SearchFilter | Logic.FilterType.SortingFilter;

        public override IColumnEvent? DoDraw((int, int)? currentValue, int rowIndex,
            FilterConfiguration filterConfiguration)
        {
            if (currentValue.HasValue && currentValue.Value.Item1 == Loading)
            {
                ImGui.TableNextColumn();
                ImGui.TextColored(ImGuiColors.DalamudYellow, LoadingString);
            }
            else if (currentValue.HasValue && currentValue.Value.Item1 == Untradable)
            {
                ImGui.TableNextColumn();
                ImGui.TextColored(ImGuiColors.DalamudRed, UntradableString);
            }
            else if(currentValue.HasValue)
            {
                base.DoDraw(currentValue, rowIndex, filterConfiguration);

            }
            else
            {
                base.DoDraw(currentValue, rowIndex, filterConfiguration);
            }

            return null;
        }

        public override (int,int)? CurrentValue(InventoryItem item)
        {
            if (!item.CanBeTraded)
            {
                return (Untradable, Untradable);
            }
            var value = base.CurrentValue(item);
            return value.HasValue ? ((int)(value.Value.Item1 * item.Quantity), (int)(value.Value.Item2 * item.Quantity)) : null;
        }

        public override (int,int)? CurrentValue(SortingResult item)
        {
            return CurrentValue(item.InventoryItem);
        }

        public override string Name { get; set; } = "MB Average Total Price";
        public override float Width { get; set; } = 250.0f;
        public override string FilterText { get; set; } = "";
        public override bool HasFilter { get; set; } = true;
        public override ColumnFilterType FilterType { get; set; } = ColumnFilterType.Text;
    }
}