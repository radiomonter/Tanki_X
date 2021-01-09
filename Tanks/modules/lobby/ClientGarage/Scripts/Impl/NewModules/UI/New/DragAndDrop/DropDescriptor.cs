namespace tanks.modules.lobby.ClientGarage.Scripts.Impl.NewModules.UI.New.DragAndDrop
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DropDescriptor
    {
        public DragAndDropCell sourceCell;
        public DragAndDropCell destinationCell;
        public DragAndDropItem item;
    }
}

