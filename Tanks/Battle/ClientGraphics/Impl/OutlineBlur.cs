namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Camera))]
    public class OutlineBlur : MonoBehaviour
    {
        [SerializeField, Range(0f, 10f)]
        private float texelSizeCoeff;
        [SerializeField]
        private Shader tmpOutline;
        [SerializeField]
        private Shader outlineBlur;
        private Material blur;
        private static RenderTexture Outline;
        private static RenderTexture BlurredOutline;
        private float oldAspectRatio;
        private float newAspectRatio;

        private void OnDisable()
        {
            Graphics.Blit(new RenderTexture(1, 1, 0), BlurredOutline);
        }

        private void OnEnable()
        {
            this.oldAspectRatio = Screen.width / Screen.height;
            Outline = new RenderTexture(Screen.width, Screen.height, 0x18);
            BlurredOutline = new RenderTexture(Screen.width >> 1, Screen.height >> 1, 0);
            Camera component = base.GetComponent<Camera>();
            component.targetTexture = Outline;
            component.SetReplacementShader(this.tmpOutline, "Outline");
            Shader.SetGlobalTexture("_OutlineUnbluredTexture", Outline);
            Shader.SetGlobalTexture("_OutlineBluredTexture", BlurredOutline);
            this.blur = new Material(this.outlineBlur);
            this.blur.SetVector("_BlurSize", new Vector2(BlurredOutline.texelSize.x * this.texelSizeCoeff, BlurredOutline.texelSize.y * this.texelSizeCoeff));
        }

        private void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            this.newAspectRatio = ((float) Screen.width) / ((float) Screen.height);
            if (this.oldAspectRatio != this.newAspectRatio)
            {
                this.OnDisable();
                this.OnEnable();
                this.oldAspectRatio = this.newAspectRatio;
            }
            this.blur.SetVector("_BlurSize", new Vector2(BlurredOutline.texelSize.x * this.texelSizeCoeff, BlurredOutline.texelSize.y * this.texelSizeCoeff));
            Graphics.Blit(src, dst);
            Graphics.Blit(src, BlurredOutline);
            for (int i = 0; i < 4; i++)
            {
                RenderTexture temporary = RenderTexture.GetTemporary(BlurredOutline.width, BlurredOutline.height);
                Graphics.Blit(BlurredOutline, temporary, this.blur, 0);
                Graphics.Blit(temporary, BlurredOutline, this.blur, 1);
                RenderTexture.ReleaseTemporary(temporary);
            }
            this.oldAspectRatio = ((float) Screen.width) / ((float) Screen.height);
        }
    }
}

