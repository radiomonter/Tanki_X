namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    public class RotationDisabled : MonoBehaviour
    {
        public void LateUpdate()
        {
            base.transform.rotation = Quaternion.identity;
        }
    }
}

