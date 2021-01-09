namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using UnityEngine;

    [SerialVersionUID(0x8d2e6e1362069caL)]
    public class CharacterShadowComponent : MonoBehaviour, Component
    {
        public float offset;
        public float attenuation = 5f;
        public float backFadeRange = 1f;
        public Color color = new Color(0f, 0f, 0f, 0.5f);
    }
}

