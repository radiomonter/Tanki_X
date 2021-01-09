namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Camera))]
    public class OutlineRender : MonoBehaviour
    {
        [SerializeField]
        private Shader outlineFinal;
        [Range(0f, 20f)]
        public float Intensity = 2f;
        private Material _compositeMat;
        [SerializeField]
        private Camera helperCamera;

        private void Awake()
        {
            this.helperCamera.gameObject.SetActive(false);
            this._compositeMat = new Material(this.outlineFinal);
        }

        private void ClearScreen(RenderTexture dst)
        {
            Graphics.Blit(new RenderTexture(1, 1, 0), dst);
        }

        private void OnDisable()
        {
            this.helperCamera.gameObject.SetActive(false);
        }

        public void OnEnable()
        {
            int qualityLevel = QualitySettings.GetQualityLevel();
            this.helperCamera.gameObject.SetActive(qualityLevel >= 2);
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            if (!this.helperCamera.gameObject.activeSelf)
            {
                this._compositeMat.SetFloat("_Intensity", 0f);
                Graphics.Blit(src, dst, this._compositeMat, 0);
            }
            else
            {
                this._compositeMat.SetFloat("_Intensity", this.Intensity);
                Graphics.Blit(src, dst, this._compositeMat, 0);
            }
        }
    }
}

