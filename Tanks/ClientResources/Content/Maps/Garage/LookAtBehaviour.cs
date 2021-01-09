namespace tanks.ClientResources.Content.Maps.Garage
{
    using System;
    using UnityEngine;

    public class LookAtBehaviour : MonoBehaviour
    {
        [Header("By default - MainCamera")]
        public Transform Target;
        public bool OnlyY;

        private void Awake()
        {
            if (this.Target == null)
            {
                this.Target = Camera.main.transform;
            }
        }

        private void Update()
        {
            base.transform.LookAt(this.Target);
            if (this.OnlyY)
            {
                Vector3 eulerAngles = base.transform.rotation.eulerAngles;
                eulerAngles.x = 0f;
                eulerAngles.z = 0f;
                base.transform.rotation = Quaternion.Euler(eulerAngles);
            }
        }
    }
}

