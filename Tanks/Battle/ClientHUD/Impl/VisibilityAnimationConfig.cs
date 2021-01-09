namespace Tanks.Battle.ClientHUD.Impl
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class VisibilityAnimationConfig : MonoBehaviour
    {
        private const string VISIBLE_ANIMATION_PROP = "Visible";
        private const string INITIALLY_VISIBLE_ANIMATION_PROP = "InitiallyVisible";
        private const string NO_ANIMATION_PROP = "NoAnimation";
        [SerializeField]
        private bool initiallyVisible;
        [SerializeField]
        private bool noAnimation;

        public void OnEnable()
        {
            Animator component = base.GetComponent<Animator>();
            component.SetBool("NoAnimation", this.noAnimation);
            component.SetBool("InitiallyVisible", this.initiallyVisible);
            component.SetBool("Visible", this.initiallyVisible);
        }
    }
}

