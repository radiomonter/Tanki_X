namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [RequireComponent(typeof(LineRenderer))]
    public class WeaponStreamTracerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float speed = 10f;
        [SerializeField]
        private float fragmentLength = 30f;
        private float textureOffset;
        private LineRenderer lineRenderer;

        private void Awake()
        {
            this.lineRenderer = base.GetComponent<LineRenderer>();
            this.lineRenderer.sharedMaterial = Instantiate<Material>(this.lineRenderer.material);
        }

        private void Update()
        {
            this.lineRenderer.SetPosition(1, this.TargetPosition);
            Vector2 mainTextureScale = this.lineRenderer.sharedMaterial.mainTextureScale;
            mainTextureScale.x = this.TargetPosition.magnitude / this.fragmentLength;
            this.lineRenderer.sharedMaterial.mainTextureScale = mainTextureScale;
            Vector2 mainTextureOffset = this.lineRenderer.sharedMaterial.mainTextureOffset;
            mainTextureOffset.x = (mainTextureOffset.x + (this.speed * Time.deltaTime)) % 1f;
            this.lineRenderer.sharedMaterial.mainTextureOffset = mainTextureOffset;
        }

        public Vector3 TargetPosition { get; set; }

        public float Speed
        {
            get => 
                this.speed;
            set => 
                this.speed = value;
        }

        public float FragmentLength
        {
            get => 
                this.fragmentLength;
            set => 
                this.fragmentLength = value;
        }
    }
}

