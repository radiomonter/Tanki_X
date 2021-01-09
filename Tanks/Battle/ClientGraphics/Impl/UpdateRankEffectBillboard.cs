namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class UpdateRankEffectBillboard : MonoBehaviour
    {
        [SerializeField]
        private Camera camera;
        [SerializeField]
        private bool active;
        [SerializeField]
        private bool autoInitCamera = true;
        private GameObject myContainer;
        private Transform cameraTransform;
        private Transform containerTransform;

        private void Awake()
        {
            if (this.autoInitCamera)
            {
                this.camera = Camera.main;
                this.active = true;
            }
            Transform parent = base.transform.parent;
            this.cameraTransform = this.camera.transform;
            GameObject obj2 = new GameObject {
                name = "Billboard_" + base.gameObject.name
            };
            this.myContainer = obj2;
            this.containerTransform = this.myContainer.transform;
            this.containerTransform.position = base.transform.position;
            base.transform.parent = this.myContainer.transform;
            this.containerTransform.parent = parent;
        }

        private void Update()
        {
            if (((this.containerTransform != null) && (this.cameraTransform != null)) && this.active)
            {
                this.containerTransform.LookAt(this.containerTransform.position + (this.cameraTransform.rotation * Vector3.back), (Vector3) (this.cameraTransform.rotation * Vector3.up));
            }
        }
    }
}

