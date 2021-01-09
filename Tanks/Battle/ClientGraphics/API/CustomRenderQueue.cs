namespace Tanks.Battle.ClientGraphics.API
{
    using System;
    using UnityEngine;

    public class CustomRenderQueue
    {
        public const int BONUS_REGION_RENDER_QUEUE = 0xc1c;
        public const int WEAPON_EFFECT_RENDER_QUEUE = 0xc4e;
        public const int DECAL_BASE_RENDER_QUEUE = 0x898;
        public const int SHAFT_AIMING_RENDER_QUEUE = 0xdac;
        public const int WATER_RENDER_QUEUE = 0x993;

        public static void SetQueue(GameObject gameObject, int queue)
        {
            foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
            {
                renderer.material.renderQueue = queue;
            }
        }
    }
}

