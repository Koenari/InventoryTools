using CriticalCommonLib.Models;
using CriticalCommonLib.Sheets;
using InventoryTools.Logic.Filters.Abstract;

namespace InventoryTools.Logic.Filters
{
    public class CanBeTradedFilter : BooleanFilter
    {
        public override string Key { get; set; } = "CanBeTraded";
        public override string Name { get; set; } = "Can be Traded?";
        public override string HelpText { get; set; } = "Can this item be traded?";
        public override FilterCategory FilterCategory { get; set; } = FilterCategory.Acquisition;

        public override FilterType AvailableIn { get; set; } =
            FilterType.SearchFilter | FilterType.SortingFilter | FilterType.GameItemFilter;
        
        public override bool? FilterItem(FilterConfiguration configuration, InventoryItem item)
        {
            var currentValue = CurrentValue(configuration);

            return currentValue == null || currentValue.Value && item.CanBeTraded || !currentValue.Value && !item.CanBeTraded;
        }

        public override bool? FilterItem(FilterConfiguration configuration, ItemEx item)
        {
            var currentValue = CurrentValue(configuration);
            return currentValue == null || currentValue.Value && item.CanBeTraded || !currentValue.Value && !item.CanBeTraded;
        }
    }
}