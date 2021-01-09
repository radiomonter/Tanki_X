namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class ReleaseGiftsPopup : ConfirmDialogComponent
    {
        public RectTransform itemsContainer;
        public ReleaseGiftItemComponent itemPrefab;
        public float itemsShowDelay = 0.6f;
        public ImageSkin leagueIcon;
        public TextMeshProUGUI headerText;
        public TextMeshProUGUI text;
    }
}

