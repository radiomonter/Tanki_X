namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class ShaftReticleBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float maxSize = 512f;
        private RectTransform transform;

        public void Init()
        {
            this.transform = base.gameObject.GetComponent<RectTransform>();
        }

        public void UpdateSize()
        {
            float b = Mathf.Min(Screen.width, Screen.height);
            float x = Mathf.Min(this.maxSize, b);
            this.transform.sizeDelta = new Vector2(x, x);
        }
    }
}

