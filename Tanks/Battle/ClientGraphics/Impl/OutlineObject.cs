namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class OutlineObject : MonoBehaviour
    {
        [SerializeField]
        private Color glowColor;
        [Range(0f, 1f)]
        public float saturation;
        public float LerpFactor = 10f;
        public bool Enable;
        private List<Material> _materials = new List<Material>();
        private Color _currentColor;
        private Color _targetColor;

        private void Start()
        {
            this.Renderers = base.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in this.Renderers)
            {
                this._materials.AddRange(renderer.materials);
            }
        }

        private void Update()
        {
            this._targetColor = !this.Enable ? Color.black : (this.glowColor * this.saturation);
            this._currentColor = Color.Lerp(this._currentColor, this._targetColor, Time.deltaTime * this.LerpFactor);
            for (int i = 0; i < this._materials.Count; i++)
            {
                this._materials[i].SetColor("_outlineColor", this._currentColor);
            }
        }

        public Color GlowColor
        {
            get => 
                this.glowColor;
            set => 
                this.glowColor = value;
        }

        public Renderer[] Renderers { get; private set; }
    }
}

