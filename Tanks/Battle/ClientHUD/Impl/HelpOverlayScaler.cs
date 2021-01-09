namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using UnityEngine;

    public class HelpOverlayScaler : MonoBehaviour
    {
        public int resolutionXScaleSize;
        public GameObject[] scaledObjects;
        public Vector3 scaleFactor;

        private void Start()
        {
            if (Screen.width <= this.resolutionXScaleSize)
            {
                foreach (GameObject obj2 in this.scaledObjects)
                {
                    obj2.transform.localScale = this.scaleFactor;
                }
            }
        }
    }
}

