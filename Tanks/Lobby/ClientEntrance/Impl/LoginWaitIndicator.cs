namespace Tanks.Lobby.ClientEntrance.Impl
{
    using System;
    using UnityEngine;

    public class LoginWaitIndicator : MonoBehaviour
    {
        public float angle = 1f;

        private void Update()
        {
            base.transform.Rotate(Vector3.back, this.angle);
        }
    }
}

