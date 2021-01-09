namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using UnityEngine;

    public class HealingGraphicEffectComponent : BaseHealingGraphicEffectComponent<StopHealingGraphicsEffectEvent>
    {
        private const string GLOBAL_EFFECT_ALPHA_KEY = "_RepairAlpha";
        private const string BACK_BORDER_COEFF_KEY = "_RepairBackCoeff";
        private const string LENGTH_EXTENSION_KEY = "_RepairAdditionalLengthExtension";
        private const string FADE_ALPHA_RANGE_KEY = "_RepairFadeAlphaRange";
        private const string MESH_SIZE_KEY = "_RepairVolume";
        private const string CENTER_OFFSET_KEY = "_RepairCenter";
        private const float MOVEMENT_DIRECTION_VALUE = 1f;
        private const float FRONT_BORDER_COEFF_VALUE = 0f;
        private const float BACK_BORDER_COEFF_VALUE = 0.69f;
        private const float LENGTH_EXTENSION_VALUE = 0.125f;
        private const float FADE_ALPHA_RANGE_VALUE = 1f;
        private const float GLOBAL_EFFECT_ALPHA_VALUE = 1f;

        public override void InitRepairGraphicsEffect(HealingGraphicEffectInputs tankInputs, WeaponHealingGraphicEffectInputs weaponInputs, Transform soundRoot, Transform mountPoint)
        {
            base.InitRepairGraphicsEffect(tankInputs, weaponInputs, soundRoot, mountPoint);
            SkinnedMeshRenderer renderer = tankInputs.Renderer;
            SkinnedMeshRenderer renderer2 = weaponInputs.Renderer;
            Bounds localBounds = renderer.localBounds;
            Bounds bounds2 = renderer2.localBounds;
            Vector3 extents = renderer.localBounds.extents;
            Vector3 vector2 = renderer2.localBounds.extents;
            this.InitTankPartInputs(tankInputs, extents, localBounds.center);
            this.InitTankPartInputs(weaponInputs, vector2, bounds2.center);
        }

        private void InitTankPartInputs(HealingGraphicEffectInputs inputs, Vector3 extents, Vector3 effectCenter)
        {
            Material[] materials = inputs.Renderer.materials;
            int length = materials.Length;
            for (int i = 0; i < length; i++)
            {
                Material mat = materials[i];
                base.SetInitialTankPartsParameters(mat);
                this.SetConstantParameters(mat);
                this.SetMeshSizeParams(extents, effectCenter, mat);
            }
        }

        private void SetConstantParameters(Material mat)
        {
            mat.SetFloat("_RepairAlpha", 1f);
            mat.SetFloat("_RepairMovementDirection", 1f);
            mat.SetFloat("_RepairFrontCoeff", 0f);
            mat.SetFloat("_RepairBackCoeff", 0.69f);
            mat.SetFloat("_RepairAdditionalLengthExtension", 0.125f);
            mat.SetFloat("_RepairFadeAlphaRange", 1f);
        }

        private void SetMeshSizeParams(Vector3 extents, Vector3 effectCenter, Material mat)
        {
            mat.SetVector("_RepairVolume", new Vector4(extents.x, extents.y, extents.z, 0f));
            mat.SetVector("_RepairCenter", new Vector4(effectCenter.x, effectCenter.y, effectCenter.z, 0f));
        }
    }
}

