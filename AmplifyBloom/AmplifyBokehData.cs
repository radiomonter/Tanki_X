namespace AmplifyBloom
{
    using System;
    using UnityEngine;

    [Serializable]
    public class AmplifyBokehData
    {
        internal RenderTexture BokehRenderTexture;
        internal Vector4[] Offsets;

        public AmplifyBokehData(Vector4[] offsets)
        {
            this.Offsets = offsets;
        }

        public void Destroy()
        {
            if (this.BokehRenderTexture != null)
            {
                AmplifyUtils.ReleaseTempRenderTarget(this.BokehRenderTexture);
                this.BokehRenderTexture = null;
            }
            this.Offsets = null;
        }
    }
}

