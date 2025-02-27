using System.Collections.Generic;
using CriticalCommonLib.Extensions;
using CriticalCommonLib.Models;
using CriticalCommonLib.Sheets;
using InventoryTools.Logic.Filters.Abstract;

namespace InventoryTools.Logic.Filters
{
    public class EquippableByGenderFilter : ChoiceFilter<uint?>
    {
        public override uint? CurrentValue(FilterConfiguration configuration)
        {
            return configuration.GetUintFilter(Key);
        }

        public override void UpdateFilterConfiguration(FilterConfiguration configuration, uint? newValue)
        {
            configuration.UpdateUintFilter(Key,newValue);
        }

        public override string Key { get; set; } = "EquippableByGender";
        public override string Name { get; set; } = "Equippable By Gender";
        public override string HelpText { get; set; } = "Which genders can this equipment be equipped by?";
        public override FilterCategory FilterCategory { get; set; } = FilterCategory.Basic;

        public override FilterType AvailableIn { get; set; } =
            FilterType.SearchFilter | FilterType.SortingFilter | FilterType.GameItemFilter;
        
        public override bool? FilterItem(FilterConfiguration configuration, InventoryItem item)
        {
            return FilterItem(configuration, item.Item) == true;
        }

        public override bool? FilterItem(FilterConfiguration configuration, ItemEx item)
        {
            var currentValue = this.CurrentValue(configuration);
            if (currentValue == null)
            {
                return true;
            }

            CharacterSex sex = (CharacterSex) currentValue;
            return item.CanBeEquippedByRaceGender(CharacterRace.Any, sex);
        }

        public override uint? EmptyValue { get; set; } = null;

        public override List<uint?> GetChoices(FilterConfiguration configuration)
        {
            var choices = new List<uint?>
            {
                (uint) CharacterSex.Both,
                (uint) CharacterSex.Female,
                (uint) CharacterSex.Male,
                (uint) CharacterSex.Either,
                (uint) CharacterSex.FemaleOnly,
                (uint) CharacterSex.MaleOnly
            };
            return choices;
        }

        public override string GetFormattedChoice(uint? choice)
        {
            if (choice == null)
            {
                return "";
            }

            return ((CharacterSex) choice).FormattedName();
        }
    }
}