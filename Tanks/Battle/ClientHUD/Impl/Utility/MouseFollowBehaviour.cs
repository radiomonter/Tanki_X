namespace Tanks.Battle.ClientHUD.Impl.Utility
{
    using System;
    using UnityEngine;

    public class MouseFollowBehaviour : MonoBehaviour
    {
        public RectTransform followObject;
        public Vector3 offset;
        public Camera uiCamera;
        public float smoothTime = 0.5f;
        private float objZ;
        private Vector3 currentVelocity;

        private void Start()
        {
            this.objZ = this.followObject.position.z;
        }

        private void Update()
        {
            if (this.uiCamera == null)
            {
                GameObject obj2 = GameObject.Find("UICamera");
                if (obj2 == null)
                {
                    return;
                }
                this.uiCamera = obj2.GetComponent<Camera>();
            }
            Vector3 position = Input.mousePosition + this.offset;
            position.z = this.objZ;
            Vector3 target = this.uiCamera.ScreenToWorldPoint(position);
            this.followObject.position = Vector3.SmoothDamp(this.followObject.position, target, ref this.currentVelocity, this.smoothTime);
        }
    }
}

