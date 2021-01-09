namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class LeagueRewardListUIComponent : MonoBehaviour
    {
        [SerializeField]
        private LeagueRewardItem itemPrefab;

        public void AddItem(string text, string imageUID)
        {
            this.GetNewItem(text, imageUID);
        }

        public void Clear()
        {
            base.transform.DestroyChildren();
        }

        private LeagueRewardItem GetNewItem(string text, string imageUID)
        {
            LeagueRewardItem item = Instantiate<LeagueRewardItem>(this.itemPrefab);
            item.transform.SetParent(base.transform, false);
            item.Text = text;
            item.Icon = imageUID;
            item.gameObject.SetActive(true);
            return item;
        }
    }
}

