using System;
using System.Collections.Generic;
using System.Linq;
using CriticalCommonLib.Models;
using CriticalCommonLib.Sheets;
using InventoryTools.Logic.Filters.Abstract;

namespace InventoryTools.Logic.Filters
{
    public class DisplayFilterInRetainersFilter : ChoiceFilter<FilterItemsRetainerEnum>
    {
        public override string Key { get; set; } = "FilterInRetainers";
        public override string Name { get; set; } = "Filter Items when in Retainer?";

        public override string HelpText { get; set; } =
            "When talking with a retainer should the filter adjust itself to only show items that should be put inside the retainer from your inventory? If set to only, highlighting will only occur when at the retainer bell and when within a retainer.";

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

        public override FilterItemsRetainerEnum CurrentValue(FilterConfiguration configuration)
        {
            return configuration.FilterItemsInRetainersEnum;
        }

        public override void UpdateFilterConfiguration(FilterConfiguration configuration, FilterItemsRetainerEnum newValue)
        {
            configuration.FilterItemsInRetainersEnum = newValue;
        }

        public override FilterItemsRetainerEnum EmptyValue { get; set; } = FilterItemsRetainerEnum.No;
        public override List<FilterItemsRetainerEnum> GetChoices(FilterConfiguration configuration)
        {
            return Enum.GetValues<FilterItemsRetainerEnum>().ToList();
        }

        public override string GetFormattedChoice(FilterItemsRetainerEnum choice)
        {
            if (choice == FilterItemsRetainerEnum.No)
            {
                return "No";
            }

            if (choice == FilterItemsRetainerEnum.Yes)
            {
                return "Yes";
            }

            if (choice == FilterItemsRetainerEnum.Only)
            {
                return "Only";
            }

            return choice.ToString();
        }
    }
}