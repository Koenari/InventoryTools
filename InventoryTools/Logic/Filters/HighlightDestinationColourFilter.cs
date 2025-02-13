using System.Numerics;
using CriticalCommonLib.Models;
using CriticalCommonLib.Sheets;
using InventoryTools.Logic.Filters.Abstract;

namespace InventoryTools.Logic.Filters
{
    public class HighlightDestinationColourFilter : ColorFilter
    {
        public override string Key { get; set; } = "HighlightDestinationColor";
        public override string Name { get; set; } = "Highlight Destination Color";

        public override string HelpText { get; set; } =
            "The color to set any items in the destination that match your source filter(assuming highlight destination duplicates is on).";

        public override FilterCategory FilterCategory { get; set; } = FilterCategory.Display;

        public override FilterType AvailableIn { get; set; } =
            FilterType.SearchFilter | FilterType.SortingFilter | FilterType.GameItemFilter;
        
        public override bool? FilterItem(FilterConfiguration configuration, InventoryItem item)
        {
            return null;
        }

        public override bool? FilterItem(FilterConfiguration configuration, ItemEx item)
        {
            return null;
        }
        
        
        public override Vector4? CurrentValue(FilterConfiguration configuration)
        {
            return configuration.HighlightColor;
        }

        public override bool HasValueSet(FilterConfiguration configuration)
        {
            return configuration.HighlightColor != null;
        }

        public override void UpdateFilterConfiguration(FilterConfiguration configuration, Vector4? newValue)
        {
            configuration.HighlightColor = newValue;
        }
    }
}