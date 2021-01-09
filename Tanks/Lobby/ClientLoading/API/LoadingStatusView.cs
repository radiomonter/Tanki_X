namespace Tanks.Lobby.ClientLoading.API
{
    using System;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class LoadingStatusView : MonoBehaviour
    {
        public LocalizedField loadFromNetworkText;
        public LocalizedField mbText;
        public LocalizedField loadFromDiskText;
        public TextMeshProUGUI loadingStatus;

        public void UpdateView(LoadBundlesTaskComponent loadBundlesTask)
        {
            if (loadBundlesTask.MBytesToLoadFromNetwork <= 5)
            {
                this.loadingStatus.gameObject.SetActive(false);
            }
            else
            {
                this.loadingStatus.gameObject.SetActive(true);
                if (loadBundlesTask.MBytesLoadedFromNetwork >= loadBundlesTask.MBytesToLoadFromNetwork)
                {
                    this.loadingStatus.text = this.loadFromDiskText.Value;
                }
                else
                {
                    this.loadingStatus.text = string.Format("{0} \n{1} {3} / {2} {3}", new object[] { this.loadFromNetworkText.Value, loadBundlesTask.MBytesLoadedFromNetwork, loadBundlesTask.MBytesToLoadFromNetwork, this.mbText.Value });
                }
            }
        }
    }
}

