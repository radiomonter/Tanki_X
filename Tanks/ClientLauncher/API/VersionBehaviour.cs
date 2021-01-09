namespace Tanks.ClientLauncher.API
{
    using System;
    using UnityEngine;

    public class VersionBehaviour : MonoBehaviour
    {
        [SerializeField]
        private string currentVersion;
        [SerializeField]
        private string distributionUrl;

        public string CurrentVersion
        {
            get => 
                this.currentVersion;
            set => 
                this.currentVersion = value;
        }

        public string DistributionUrl
        {
            get => 
                this.distributionUrl;
            set => 
                this.distributionUrl = value;
        }
    }
}

