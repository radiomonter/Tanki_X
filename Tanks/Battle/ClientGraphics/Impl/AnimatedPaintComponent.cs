namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class AnimatedPaintComponent : MonoBehaviour
    {
        [SerializeField]
        private float saturtion = 1f;
        [SerializeField]
        private float value = 1f;
        [SerializeField]
        private float speed = 1f;
        private float hue = 1f;
        private List<Material> materials = new List<Material>();

        public void AddMaterial(Material material)
        {
            this.materials.Add(material);
        }

        private void Start()
        {
            this.hue = Random.Range((float) 0f, (float) 1f);
        }

        private void Update()
        {
            Color color = Color.HSVToRGB(this.hue, this.saturtion, this.value);
            this.hue += Time.deltaTime * this.speed;
            if (this.hue > 1f)
            {
                this.hue = 0f;
            }
            foreach (Material material in this.materials)
            {
                material.SetColor(TankMaterialPropertyNames.COLORING_ID, color);
            }
        }
    }
}

