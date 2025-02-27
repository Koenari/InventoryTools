using System.Collections.Generic;
using System.Linq;
using CriticalCommonLib.Extensions;
using CriticalCommonLib.Models;
using CriticalCommonLib.Sheets;
using InventoryTools.Logic.Filters.Abstract;

namespace InventoryTools.Logic.Filters
{
    public class DestinationInventoriesFilter : MultipleChoiceFilter<(ulong, InventoryCategory)>
    {
        public override int LabelSize { get; set; } = 240;
        public override string Key { get; set; } = "DestinationInventories";
        public override string Name { get; set; } = "Destination - Inventories";
        public override string HelpText { get; set; } =
            "This is a list of destinations to sort items into.";
        public override FilterCategory FilterCategory { get; set; } = FilterCategory.Inventories;
        public override FilterType AvailableIn { get; set; } = FilterType.SortingFilter | FilterType.CraftFilter;
        
        public override bool? FilterItem(FilterConfiguration configuration, InventoryItem item)
        {
            return null;
        }

        public override bool? FilterItem(FilterConfiguration configuration, ItemEx item)
        {
            return null;
        }

        public override Dictionary<(ulong, InventoryCategory), string> GetChoices(FilterConfiguration configuration)
        {
            var allCharacters = PluginService.CharacterMonitor.AllCharacters();
            if (!PluginLogic.PluginConfiguration.DisplayCrossCharacter)
            {
                allCharacters = allCharacters.Where(c =>
                    PluginService.CharacterMonitor.BelongsToActiveCharacter(c.Key)).ToArray();
            }
            
            var dict = new Dictionary<(ulong, InventoryCategory), string>();
            foreach (var character in allCharacters)
            {
                if (PluginService.CharacterMonitor.IsRetainer(character.Key))
                {
                    dict.Add((character.Key, InventoryCategory.RetainerBags), character.Value.FormattedName + " - " + InventoryCategory.RetainerBags.FormattedName());
                    dict.Add((character.Key, InventoryCategory.RetainerMarket), character.Value.FormattedName + " - " + InventoryCategory.RetainerMarket.FormattedName());
                    dict.Add((character.Key, InventoryCategory.RetainerEquipped), character.Value.FormattedName + " - " + InventoryCategory.RetainerEquipped.FormattedName());
                    dict.Add((character.Key, InventoryCategory.Currency), character.Value.FormattedName + " - " + InventoryCategory.Currency.FormattedName());
                    dict.Add((character.Key, InventoryCategory.Crystals), character.Value.FormattedName + " - " + InventoryCategory.Crystals.FormattedName());
                }
                else
                {
                    dict.Add((character.Key, InventoryCategory.CharacterBags), character.Value.FormattedName + " - " + InventoryCategory.CharacterBags.FormattedName());
                    dict.Add((character.Key, InventoryCategory.CharacterSaddleBags), character.Value.FormattedName + " - " + InventoryCategory.CharacterSaddleBags.FormattedName());
                    dict.Add((character.Key, InventoryCategory.CharacterPremiumSaddleBags), character.Value.FormattedName + " - " + InventoryCategory.CharacterPremiumSaddleBags.FormattedName());
                    dict.Add((character.Key, InventoryCategory.FreeCompanyBags), character.Value.FormattedName + " - " + InventoryCategory.FreeCompanyBags.FormattedName());
                    dict.Add((character.Key, InventoryCategory.CharacterArmoryChest), character.Value.FormattedName + " - " + InventoryCategory.CharacterArmoryChest.FormattedName());
                    dict.Add((character.Key, InventoryCategory.GlamourChest), character.Value.FormattedName + " - " + InventoryCategory.GlamourChest.FormattedName());
                    dict.Add((character.Key, InventoryCategory.Armoire), character.Value.FormattedName + " - " + InventoryCategory.Armoire.FormattedName());
                    dict.Add((character.Key, InventoryCategory.Crystals), character.Value.FormattedName + " - " + InventoryCategory.Crystals.FormattedName());
                    dict.Add((character.Key, InventoryCategory.Currency), character.Value.FormattedName + " - " + InventoryCategory.Currency.FormattedName());
                    dict.Add((character.Key, InventoryCategory.CharacterEquipped), character.Value.FormattedName + " - " + InventoryCategory.CharacterEquipped.FormattedName());
                   
                }
            }

            return dict;
        }

        public override List<(ulong, InventoryCategory)> CurrentValue(FilterConfiguration configuration)
        {
            return configuration.DestinationInventories.ToList();
        }

        public override void UpdateFilterConfiguration(FilterConfiguration configuration, List<(ulong, InventoryCategory)> newValue)
        {
            List<(ulong, InventoryCategory)> newDestinationInventories = new();
            foreach (var item in newValue)
            {
                newDestinationInventories.Add(item);
            }

            configuration.DestinationInventories = newDestinationInventories;
        }

        public override bool HideAlreadyPicked { get; set; } = true;
    }
}