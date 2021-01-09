namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientUserProfile.API;
    using UnityEngine;

    public class NewsContainerComponent : BehaviourComponent
    {
        public GameObject newsItemPrefab;
        public RectTransform smallItems;
        public RectTransform mediumItems;
        public RectTransform largeItems;
        public RectTransform row1;
        public RectTransform row2;

        public Transform GetContainerTransform(NewsItemLayout layout) => 
            (layout != NewsItemLayout.SMALL) ? ((layout != NewsItemLayout.MEDIUM) ? ((layout != NewsItemLayout.LARGE) ? null : this.largeItems) : this.mediumItems) : this.smallItems;

        private void Update()
        {
            bool flag = false;
            bool flag2 = false;
            if (this.smallItems.childCount <= 0)
            {
                this.smallItems.gameObject.SetActive(false);
            }
            else
            {
                this.smallItems.gameObject.SetActive(true);
                flag = true;
            }
            if (this.mediumItems.childCount <= 0)
            {
                this.mediumItems.gameObject.SetActive(false);
            }
            else
            {
                this.mediumItems.gameObject.SetActive(true);
                flag = true;
            }
            if (this.largeItems.childCount <= 0)
            {
                this.largeItems.gameObject.SetActive(false);
            }
            else
            {
                this.largeItems.gameObject.SetActive(true);
                flag2 = true;
            }
            this.row1.gameObject.SetActive(flag);
            this.row2.gameObject.SetActive(flag2);
        }
    }
}

