namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class HealthFeedbackPostEffect : MonoBehaviour
    {
        private const float MAX_DAMAGE_LEVEL = 100f;
        [SerializeField]
        private Material mat;
        private int damageID;
        private float damageIntensity;

        public void Init(Material sourceMaterial)
        {
            this.mat = Instantiate<Material>(sourceMaterial);
            this.damageID = Shader.PropertyToID("_damage_lvl");
            this.DamageIntensity = 0f;
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Graphics.Blit(src, dest, this.mat);
        }

        public float DamageIntensity
        {
            get => 
                this.damageIntensity;
            set
            {
                this.damageIntensity = value;
                this.mat.SetFloat(this.damageID, Mathf.Lerp(0f, 100f, this.damageIntensity));
            }
        }
    }
}

