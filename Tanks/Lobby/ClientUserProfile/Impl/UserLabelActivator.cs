namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Library.ClientUnityIntegration;
    using System;
    using UnityEngine;

    public class UserLabelActivator : UnityAwareActivator<AutoCompleting>
    {
        [SerializeField]
        public GameObject UserLabel;

        protected override void Activate()
        {
            if (this.UserLabel == null)
            {
                Debug.LogError("UserLabelActivator: not set prefab UserLabel");
            }
            else
            {
                UserLabelBuilder.userLabelPrefab = this.UserLabel;
            }
        }
    }
}

