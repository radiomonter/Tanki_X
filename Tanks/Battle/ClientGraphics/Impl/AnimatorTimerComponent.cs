namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class AnimatorTimerComponent : MonoBehaviour, Component
    {
        public Animator animator;
        public string triggerName;
        public float timer;
    }
}

