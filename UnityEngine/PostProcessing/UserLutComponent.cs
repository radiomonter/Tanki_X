namespace UnityEngine.PostProcessing
{
    using System;
    using UnityEngine;

    public sealed class UserLutComponent : PostProcessingComponentRenderTexture<UserLutModel>
    {
        public void OnGUI()
        {
            UserLutModel.Settings settings = base.model.settings;
            Rect position = new Rect((base.context.viewport.x * Screen.width) + 8f, 8f, (float) settings.lut.width, (float) settings.lut.height);
            GUI.DrawTexture(position, settings.lut);
        }

        public override void Prepare(Material uberMaterial)
        {
            UserLutModel.Settings settings = base.model.settings;
            uberMaterial.EnableKeyword("USER_LUT");
            uberMaterial.SetTexture(Uniforms._UserLut, settings.lut);
            uberMaterial.SetVector(Uniforms._UserLut_Params, new Vector4(1f / ((float) settings.lut.width), 1f / ((float) settings.lut.height), settings.lut.height - 1f, settings.contribution));
        }

        public override bool active
        {
            get
            {
                UserLutModel.Settings settings = base.model.settings;
                return ((base.model.enabled && ((settings.lut != null) && ((settings.contribution > 0f) && (settings.lut.height == ((int) Mathf.Sqrt((float) settings.lut.width)))))) && !base.context.interrupted);
            }
        }

        private static class Uniforms
        {
            internal static readonly int _UserLut = Shader.PropertyToID("_UserLut");
            internal static readonly int _UserLut_Params = Shader.PropertyToID("_UserLut_Params");
        }
    }
}

