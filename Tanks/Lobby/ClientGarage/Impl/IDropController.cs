namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;

    public interface IDropController
    {
        void OnDrop(DragAndDropCell cellFrom, DragAndDropCell cellTo, DragAndDropItem item);
    }
}

