namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    public interface ILazyList
    {
        void ClearItems();
        RectTransform GetItemContent(int itemIndex);
        void Scroll(int deltaItems);
        void SetItemContent(int itemIndex, RectTransform content);

        int ItemsCount { get; set; }

        IndexRange VisibleItemsRange { get; }
    }
}

