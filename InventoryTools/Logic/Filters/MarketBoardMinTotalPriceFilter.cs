using CriticalCommonLib.MarketBoard;
using CriticalCommonLib.Models;
using CriticalCommonLib.Sheets;
using InventoryTools.Extensions;

namespace InventoryTools.Logic.Filters
{
    public class MarketBoardMinTotalPriceFilter : MarketBoardTotalPriceFilter
    {
        public override string Key { get; set; } = "MBMinTotalPrice";
        public override string Name { get; set; } = "Market Board Total Minimum Price";
        public override string HelpText { get; set; } = "The total market board price of the item(minimum price * quantity). For this to work you need to have automatic pricing enabled and also note that any background price updates will not be evaluated until an event that refreshes the inventory occurs(this happens fairly often).";
        public override FilterCategory FilterCategory { get; set; } = FilterCategory.Market;

        public override FilterType AvailableIn { get; set; } =
            FilterType.SearchFilter | FilterType.SortingFilter;

        public override bool? FilterItem(FilterConfiguration configuration,InventoryItem item)
        {
            var currentValue = CurrentValue(configuration);
            if (!string.IsNullOrEmpty(currentValue))
            {
                if (!item.CanBeTraded)
                {
                    return false;
                }
                var marketBoardData = PluginService.MarketCache.GetPricing(item.ItemId, false);
                if (marketBoardData != null)
                {
                    float price;
                    if (item.IsHQ)
                    {
                        price = marketBoardData.minPriceHQ;
                    }
                    else
                    {
                        price = marketBoardData.minPriceNQ;
                    }

                    price *= item.Quantity;
                    return price.PassesFilter(currentValue.ToLower());
                }

                return false;
            }

            return true;
        }

        public override bool? FilterItem(FilterConfiguration configuration, ItemEx item)
        {
            return true;
        }
    }
}