namespace tanks.modules.lobby.ClientGarage.Scripts.Impl.NewModules.UI.New.DragAndDrop
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;

    public class DefaultDropController : IDropController
    {
        public void OnDrop(DragAndDropCell cellFrom, DragAndDropCell cellTo, DragAndDropItem item)
        {
            if ((item != null) && (cellFrom != cellTo))
            {
                cellTo.SwapItems(cellFrom, item);
            }
        }
    }
}

