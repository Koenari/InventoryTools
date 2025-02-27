using System.Numerics;
using ImGuiNET;
using InventoryTools.Logic;

namespace InventoryTools.Ui
{
    public abstract class Window : Dalamud.Interface.Windowing.Window, IWindow
    {
        protected Window(string name, ImGuiWindowFlags flags = ImGuiWindowFlags.None, bool forceMainWindow = false) : base(name, flags, forceMainWindow)
        {
            SizeConstraints = new WindowSizeConstraints()
            {
                MinimumSize = MinSize,
                MaximumSize = MaxSize
            };
            SizeCondition = ImGuiCond.FirstUseEver;
            Size = DefaultSize;
        }

        public override void OnOpen()
        {
            Opened?.Invoke(Key);
        }

        public override void OnClose()
        {
            Closed?.Invoke(Key);
        }

        public void Close()
        {
            IsOpen = false;
        }

        public void Open()
        {
            IsOpen = true;
        }

        public void Toggle()
        {
            IsOpen = !IsOpen;
        }

        public abstract void Invalidate();
        
        public abstract FilterConfiguration? SelectedConfiguration { get; }

        public event IWindow.ClosedDelegate? Closed;
        public event IWindow.OpenedDelegate? Opened;

        public abstract string Key { get; }

        public abstract bool DestroyOnClose { get; }
        public virtual ImGuiWindowFlags? WindowFlags { get; } = null;

        public abstract bool SaveState { get; }
        
        public abstract Vector2 DefaultSize { get; }
        public abstract Vector2 MaxSize { get; }
        public abstract Vector2 MinSize { get; }
    }
}