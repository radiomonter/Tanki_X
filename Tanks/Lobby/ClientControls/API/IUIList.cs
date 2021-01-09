namespace Tanks.Lobby.ClientControls.API
{
    using System;

    public interface IUIList
    {
        void AddItem(object data);
        void ClearItems();
        void RemoveItem(object data);
    }
}

