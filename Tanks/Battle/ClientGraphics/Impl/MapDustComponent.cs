namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    [SkipExceptionOnAddRemove]
    public class MapDustComponent : BehaviourComponent
    {
        public DustEffectBehaviour DefaultDust;
        public DustEffectBehaviour LowGraphicDust;
        public Quality.QualityLevel QualityForLowDust = Quality.QualityLevel.Minimum;
        public int ParentDeep;
        public MapDustEffect[] Targets;
        private readonly Dictionary<Transform, TargetEffects> _effects = new Dictionary<Transform, TargetEffects>();

        public DustEffectBehaviour GetEffectByTag(Transform target, Vector2 uv)
        {
            if ((QualitySettings.GetQualityLevel() <= this.QualityForLowDust) || !target)
            {
                return this.LowGraphicDust;
            }
            int num2 = 0;
            bool flag = this._effects.ContainsKey(target);
            while (true)
            {
                DustEffectBehaviour behaviour;
                if (!flag && (this.ParentDeep > num2))
                {
                    target = target.parent;
                    if (target)
                    {
                        flag = this._effects.ContainsKey(target);
                        continue;
                    }
                }
                if (!flag)
                {
                    return this.DefaultDust;
                }
                int num3 = 0;
                TargetEffects effects = this._effects[target];
                using (List<Texture2D>.Enumerator enumerator = effects.Textures.GetEnumerator())
                {
                    while (true)
                    {
                        if (enumerator.MoveNext())
                        {
                            Texture2D current = enumerator.Current;
                            if (!current)
                            {
                                behaviour = effects.DustEffects[num3];
                            }
                            else
                            {
                                float grayscale = current.GetPixelBilinear(uv.x, uv.y).grayscale;
                                Vector2 vector = effects.GrayscaleRanges[num3];
                                if ((grayscale < vector.x) || (grayscale > vector.y))
                                {
                                    num3++;
                                    continue;
                                }
                                behaviour = effects.DustEffects[num3];
                            }
                        }
                        else
                        {
                            break;
                        }
                        break;
                    }
                }
                return behaviour;
            }
        }

        private void Start()
        {
            if (this.DefaultDust == null)
            {
                foreach (DustEffectBehaviour behaviour in FindObjectsOfType<DustEffectBehaviour>())
                {
                    if (behaviour.surface == DustEffectBehaviour.SurfaceType.Concrete)
                    {
                        this.DefaultDust = behaviour;
                        break;
                    }
                    this.DefaultDust = behaviour;
                }
            }
            if (this.LowGraphicDust == null)
            {
                this.LowGraphicDust = this.DefaultDust;
            }
            this._effects.Clear();
            foreach (MapDustEffect effect in this.Targets)
            {
                Transform target = effect.Target;
                if (target)
                {
                    TargetEffects effects = new TargetEffects();
                    MaskToDustEffect[] dustEffects = effect.DustEffects;
                    int index = 0;
                    while (true)
                    {
                        if (index >= dustEffects.Length)
                        {
                            this._effects.Add(target, effects);
                            break;
                        }
                        MaskToDustEffect effect2 = dustEffects[index];
                        effects.GrayscaleRanges.Add(effect2.GrayScaleRange);
                        effects.Textures.Add(effect2.Mask);
                        effects.DustEffects.Add(effect2.EffectBehaviour);
                        index++;
                    }
                }
            }
        }

        [Serializable]
        public class MapDustEffect
        {
            public Transform Target;
            public MapDustComponent.MaskToDustEffect[] DustEffects;
        }

        [Serializable]
        public class MaskToDustEffect
        {
            [Tooltip("Min and Max Grayscale")]
            public Vector2 GrayScaleRange;
            public Texture2D Mask;
            public DustEffectBehaviour EffectBehaviour;
        }

        private class TargetEffects
        {
            public readonly List<Vector2> GrayscaleRanges = new List<Vector2>();
            public readonly List<Texture2D> Textures = new List<Texture2D>();
            public readonly List<DustEffectBehaviour> DustEffects = new List<DustEffectBehaviour>();
        }
    }
}

