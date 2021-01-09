namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    [RequireComponent(typeof(UnityEngine.Animator))]
    public class AnimationComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private UnityEngine.Animator animator;

        public UnityEngine.Animator Animator
        {
            get => 
                this.animator;
            set => 
                this.animator = value;
        }
    }
}

