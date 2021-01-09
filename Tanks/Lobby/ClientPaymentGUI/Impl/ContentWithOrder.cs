namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using System;
    using UnityEngine;

    public interface ContentWithOrder
    {
        void SetParent(Transform parent);

        int Order { get; }

        bool CanFillBigRow { get; }

        bool CanFillSmallRow { get; }
    }
}

