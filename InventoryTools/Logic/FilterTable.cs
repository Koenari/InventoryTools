﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using CriticalCommonLib.Sheets;
using CsvHelper;
using Dalamud.Logging;
using ImGuiNET;
using InventoryTools.Logic.Columns;

namespace InventoryTools.Logic
{
    public class FilterTable : RenderTableBase
    {
        public FilterTable(FilterConfiguration filterConfiguration) : base(filterConfiguration)
        {
            
        }

        public override event PreFilterSortedItemsDelegate? PreFilterSortedItems;
        
        public override event PreFilterItemsDelegate? PreFilterItems;
        public override event ChangedDelegate? Refreshed;

        public override void Refresh(InventoryToolsConfiguration configuration)
        {
            //Do something with unsortable items
            if (FilterConfiguration.FilterResult != null)
            {
                if (FilterConfiguration.FilterType == FilterType.SearchFilter 
                    || FilterConfiguration.FilterType == FilterType.SortingFilter 
                    || FilterConfiguration.FilterType == FilterType.CraftFilter)
                {
                    PluginLog.Verbose("FilterTable: Refreshing");
                    var items = FilterConfiguration.FilterResult.Value.SortedItems.AsEnumerable();
                    items = PreFilterSortedItems != null ? PreFilterSortedItems.Invoke(items) : items;
                    IsSearching = false;
                    for (var index = 0; index < Columns.Count; index++)
                    {
                        var column = Columns[index];
                        if (column.FilterText != "")
                        {
                            IsSearching = true;
                        }

                        items = column.Filter(items);
                        if (SortColumn != null && index == SortColumn)
                        {
                            items = column.Sort(SortDirection ?? ImGuiSortDirection.None, items);
                        }
                    }

                    SortedItems = items.ToList();
                    RenderSortedItems = SortedItems.Where(item => !item.InventoryItem.IsEmpty).ToList();
                    NeedsRefresh = false;
                    Refreshed?.Invoke(this);
                }
                else
                {
                    PluginLog.Verbose("FilterTable: Refreshing");
                    var items = FilterConfiguration.FilterResult.Value.AllItems.AsEnumerable();
                    items = PreFilterItems != null ? PreFilterItems.Invoke(items) : items;
                    IsSearching = false;
                    for (var index = 0; index < Columns.Count; index++)
                    {
                        var column = Columns[index];
                        if (column.FilterText != "")
                        {
                            IsSearching = true;
                        }

                        items = column.Filter((IEnumerable<ItemEx>)items);
                        if (SortColumn != null && index == SortColumn)
                        {
                            items = column.Sort(SortDirection ?? ImGuiSortDirection.None, (IEnumerable<ItemEx>)items);
                        }
                    }

                    Items = items.Where(c => c.NameString.ToString() != "").ToList();
                    RenderItems = Items.ToList();
                    NeedsRefresh = false;
                    Refreshed?.Invoke(this);
                }
            }
        }

        public override void RefreshColumns()
        {
            if (FilterConfiguration.FreezeColumns != FreezeCols)
            {
                FreezeCols = FilterConfiguration.FreezeColumns;
            }
            if (FilterConfiguration.Columns != null)
            {
                var newColumns = new List<IColumn>();
                foreach (var column in FilterConfiguration.Columns)
                {
                    var newColumn = PluginLogic.GetClassFromString(column);
                    if (newColumn != null && newColumn is IColumn)
                    {
                        newColumns.Add(newColumn);
                    }
                }

                this.Columns = newColumns;
            }
        }



        public override bool Draw(Vector2 size)
        {
            var highlightItems = HighlightItems;

            
            if (Columns.Count == 0)
            {
                if (NeedsRefresh)
                {
                    Refresh(ConfigurationManager.Config);
                }
                return true;
            }

            var isExpanded = false;
            ImGui.BeginChild("FilterTableContent", size * ImGui.GetIO().FontGlobalScale, false, ImGuiWindowFlags.HorizontalScrollbar); 
            
            if((FilterConfiguration.FilterType != FilterType.CraftFilter || FilterConfiguration.FilterType == FilterType.CraftFilter && ImGui.CollapsingHeader("Items in Retainers/Bags", ImGuiTreeNodeFlags.DefaultOpen)) && ImGui.BeginTable(Key, Columns.Count, _tableFlags))
            {
                isExpanded = true;
                var refresh = false;
                ImGui.TableSetupScrollFreeze(Math.Min(FreezeCols ?? 0,Columns.Count), FreezeRows ?? (ShowFilterRow ? 2 : 1));
                for (var index = 0; index < Columns.Count; index++)
                {
                    var column = Columns[index];
                    column.Setup(index);
                }
                ImGui.TableHeadersRow();

                var currentSortSpecs = ImGui.TableGetSortSpecs();
                if (currentSortSpecs.SpecsDirty)
                {
                    var actualSpecs = currentSortSpecs.Specs;
                    if (SortColumn != actualSpecs.ColumnIndex)
                    {
                        SortColumn = actualSpecs.ColumnIndex;
                        refresh = true;
                    }

                    if (SortDirection != actualSpecs.SortDirection)
                    {
                        SortDirection = actualSpecs.SortDirection;
                        refresh = true;
                    }
                }
                else
                {
                    if (SortColumn != null)
                    {
                        SortColumn = null;
                        refresh = true;
                    }

                    if (SortDirection != null)
                    {
                        SortDirection = null;
                        refresh = true;
                    }

                }

                if (ShowFilterRow)
                {
                    ImGui.TableNextRow(ImGuiTableRowFlags.Headers);
                    foreach (var column in Columns)
                    {
                        column.SetupFilter(Key);
                    }

                    for (var index = 0; index < Columns.Count; index++)
                    {
                        var column = Columns[index];
                        if (column.HasFilter && column.DrawFilter(Key, index))
                        {
                            refresh = true;
                        }
                    }
                }

                if (refresh || NeedsRefresh)
                {
                    Refresh(ConfigurationManager.Config);
                }
                
                if (FilterConfiguration.FilterType == FilterType.SearchFilter ||
                    FilterConfiguration.FilterType == FilterType.SortingFilter ||
                    FilterConfiguration.FilterType == FilterType.CraftFilter)
                {
                    _clipper.Begin(RenderSortedItems.Count);
                    while (_clipper.Step())
                    {
                        for (var index = _clipper.DisplayStart; index < _clipper.DisplayEnd; index++)
                        {
                            var item = RenderSortedItems[index];
                            ImGui.TableNextRow(ImGuiTableRowFlags.None, FilterConfiguration.TableHeight);
                            ImGui.PushID(index);
                            for (var columnIndex = 0; columnIndex < Columns.Count; columnIndex++)
                            {
                                var column = Columns[columnIndex];
                                column.Draw(FilterConfiguration, item, index);
                                ImGui.SameLine();
                                if (columnIndex == Columns.Count - 1)
                                {
                                    PluginService.PluginLogic.RightClickColumn.Draw(FilterConfiguration, item, index);
                                }
                            }
                            ImGui.PopID();
                        }
                    }

                    _clipper.End();
                }
                else
                {
                    _clipper.Begin(RenderItems.Count);
                    while (_clipper.Step())
                    {
                        for (var index = _clipper.DisplayStart; index < _clipper.DisplayEnd; index++)
                        {
                            var item = RenderItems[index];
                            ImGui.TableNextRow(ImGuiTableRowFlags.None, FilterConfiguration.TableHeight);
                            for (var columnIndex = 0; columnIndex < Columns.Count; columnIndex++)
                            {
                                var column = Columns[columnIndex];
                                column.Draw(FilterConfiguration, (ItemEx)item, index);
                                ImGui.SameLine();
                                if (columnIndex == Columns.Count - 1)
                                {
                                    PluginService.PluginLogic.RightClickColumn.Draw(FilterConfiguration, (ItemEx)item, index);
                                }
                            }
                        }
                    }
                    _clipper.End();
                }
                ImGui.EndTable();
            }
            ImGui.EndChild();
            return isExpanded;
        }

        public override void DrawFooterItems()
        {
            foreach (var column in Columns)
            {
                var result = column.DrawFooterFilter(FilterConfiguration);
                if (result != null)
                {
                    result.HandleEvent(FilterConfiguration);
                }
            }
        }

        public void SaveCallback(bool arg1, string arg2)
        {
            if (arg1)
            {
                ExportToCsv(arg2);
            }
        }

        public void ExportToCsv(string fileName)
        {
            using (var writer = new StreamWriter(fileName))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                foreach (var column in Columns)
                {
                    csv.WriteField(column.Name);
                }
                csv.NextRecord();
                if (FilterConfiguration.FilterType == FilterType.SearchFilter ||
                    FilterConfiguration.FilterType == FilterType.SortingFilter ||
                    FilterConfiguration.FilterType == FilterType.CraftFilter)
                {
                    foreach (var item in RenderSortedItems)
                    {
                        foreach (var column in Columns)
                        {
                            csv.WriteField(column.CsvExport(item));
                        }
                        csv.NextRecord();
                    }
                }
            }

        }

        public void ClearFilters()
        {
            Columns.ForEach(c => c.FilterText = "");
            NeedsRefresh = true;
        }

        public override void Dispose()
        {
            base.Dispose();
            unsafe
            {
                _clipper.Destroy();
            }
        }
    }
}