using System.Collections.Generic;
using Dalamud.Interface.Colors;
using ImGuiNET;

namespace InventoryTools.Logic.Filters.Abstract
{
    public abstract class ChoiceFilter<T> : Filter<T?>
    {
        public abstract T EmptyValue { get; set; }
        public override bool HasValueSet(FilterConfiguration configuration)
        {
            var keyValuePair = CurrentValue(configuration);
            return keyValuePair != null && !Equals(keyValuePair, EmptyValue);
        }

        public abstract List<T> GetChoices(FilterConfiguration configuration);

        public abstract string GetFormattedChoice(T choice);

        public override void Draw(FilterConfiguration configuration)
        {
            ImGui.SetNextItemWidth(LabelSize);
            if (HasValueSet(configuration))
            {
                ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);
                ImGui.LabelText("##" + Key + "Label", Name + ":");
                ImGui.PopStyleColor();
            }
            else
            {
                ImGui.LabelText("##" + Key + "Label", Name + ":");
            }

            var choices = GetChoices(configuration);
            var activeChoice = CurrentValue(configuration);

            var currentSearchCategory = activeChoice != null ? GetFormattedChoice(activeChoice) : "";
            ImGui.SameLine();
            if (ImGui.BeginCombo("##" + Key + "Combo", currentSearchCategory))
            {
                foreach (var item in choices)
                {
                    if (item == null)
                    {
                        continue;
                    }
                    var text = GetFormattedChoice(item).Replace("\u0002\u001F\u0001\u0003", "-");
                    if (text == "")
                    {
                        continue;
                    }                    

                    if (ImGui.Selectable(text,currentSearchCategory == text))
                    {
                        UpdateFilterConfiguration(configuration,item);
                    }
                }

                ImGui.EndCombo();
            }

            ImGui.SameLine();
            UiHelpers.HelpMarker(HelpText);
        }
    }
}