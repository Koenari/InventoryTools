using CriticalCommonLib.MarketBoard;
using CriticalCommonLib.Models;
using Dalamud.Interface.Colors;
using Dalamud.Logging;
using ImGuiNET;
using InventoryTools.Extensions;
using InventoryTools.Logic.Columns.Abstract;
using Lumina.Excel.GeneratedSheets;

namespace InventoryTools.Logic.Columns
{
    public class MarketBoardSevenDayCountColumn : IntegerColumn
    {
        protected static readonly string LoadingString = "loading...";
        protected static readonly string UntradableString = "untradable";
        protected static readonly int Loading = -1;
        protected static readonly int Untradable = -2;
        
        public override IColumnEvent? DoDraw(int? currentValue, int rowIndex)
        {
            if (currentValue.HasValue && currentValue.Value == Loading)
            {
                ImGui.TableNextColumn();
                ImGui.TextColored(ImGuiColors.DalamudYellow, LoadingString);
            }
            else if (currentValue.HasValue && currentValue.Value == Untradable)
            {
                ImGui.TableNextColumn();
                ImGui.TextColored(ImGuiColors.DalamudRed, UntradableString);
            }
            else if(currentValue.HasValue)
            {
                base.DoDraw(currentValue, rowIndex);

            }
            else
            {
                base.DoDraw(currentValue, rowIndex);
            }

            return null;
        }


        public override int? CurrentValue(InventoryItem item)
        {
            if (!item.CanBeTraded)
            {
                return Untradable;
            }

            var marketBoardData = Cache.GetPricing(item.ItemId, false);
            if (marketBoardData != null)
            {
                var sevenDaySellCount = marketBoardData.sevenDaySellCount;
                return sevenDaySellCount;
            }

            return Loading;
        }

        public override int? CurrentValue(Item item)
        {
            if (!item.CanBeTraded())
            {
                return Untradable;
            }

            var marketBoardData = Cache.GetPricing(item.RowId, false);
            if (marketBoardData != null)
            {
                return marketBoardData.sevenDaySellCount;
            }

            return Loading;
        }

        public override int? CurrentValue(SortingResult item)
        {
            return CurrentValue(item.InventoryItem);
        }

        public override string Name { get; set; } = "MB Seven Day Sale Count";
        public override float Width { get; set; } = 250.0f;
        public override string FilterText { get; set; } = "";
        public override bool HasFilter { get; set; } = true;
        public override ColumnFilterType FilterType { get; set; } = ColumnFilterType.Text;
    }
}