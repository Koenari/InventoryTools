using System.Collections.Generic;
using CriticalCommonLib.Models;
using CriticalCommonLib.Sheets;

namespace InventoryTools.Logic.Filters
{
    public interface IFilter
    {
        public int LabelSize { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string HelpText { get; set; }
        
        public FilterCategory FilterCategory { get; set; }
        
        public int Order { get; set; }

        public bool HasValueSet(FilterConfiguration configuration);
        
        public FilterType AvailableIn { get; set; }
        public bool? FilterItem(FilterConfiguration configuration, InventoryItem item);
        public bool? FilterItem(FilterConfiguration configuration, ItemEx item);
        public void Draw(FilterConfiguration configuration);

        public static readonly List<FilterCategory> FilterCategoryOrder = new() {FilterCategory.Basic, FilterCategory.Columns,FilterCategory.CraftColumns, FilterCategory.Inventories, FilterCategory.Display, FilterCategory.Acquisition, FilterCategory.Searching, FilterCategory.Market, FilterCategory.Searching, FilterCategory.Crafting, FilterCategory.Gathering, FilterCategory.Advanced};
    }
}