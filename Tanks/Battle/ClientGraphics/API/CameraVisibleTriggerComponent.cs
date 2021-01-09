namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CameraVisibleTriggerComponent : BehaviourComponent
    {
        public bool IsVisibleAtRange(float testRange) => 
            this.IsVisible && (this.DistanceToCamera < testRange);

        private void OnBecameInvisible()
        {
            this.IsVisible = false;
        }

        private void OnBecameVisible()
        {
            this.IsVisible = true;
        }

        public Transform MainCameraTransform { get; set; }

        public bool IsVisible { get; set; }

        public float DistanceToCamera =>
            (this.MainCameraTransform == null) ? 0f : Vector3.Distance(this.MainCameraTransform.position, base.gameObject.transform.position);
    }
}

