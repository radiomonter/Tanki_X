namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class EffectArrangementSystem : ECSSystem
    {
        private const string EFFECT_ROOT_NAME = "Graphic Effects";
        private GameObject allEffectsRoot = GameObject.Find("Graphic Effects");

        public EffectArrangementSystem()
        {
            if (this.allEffectsRoot == null)
            {
                this.allEffectsRoot = new GameObject();
                this.allEffectsRoot.name = "Graphic Effects";
            }
        }

        [OnEventComplete]
        public void OnArrangeGraphicEffectEvent(ArrangeGraphicEffectEvent evt, GraphicEffectArrangementNode graphicEffect)
        {
            graphicEffect.graphicEffect.EffectObject.transform.parent = this.allEffectsRoot.transform;
        }

        public class GraphicEffectArrangementNode : Node
        {
            public GraphicEffectComponent graphicEffect;
        }
    }
}

