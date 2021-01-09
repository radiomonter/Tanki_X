namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [RequireComponent(typeof(CanvasGroup)), RequireComponent(typeof(UnityEngine.Animator))]
    public class VisibilityPrerequisitesComponent : BehaviourComponent, AttachToEntityListener
    {
        private HashSet<string> showPrerequisites = new HashSet<string>();
        private HashSet<string> hidePrerequisites = new HashSet<string>();
        private UnityEngine.Animator animator;

        public void AddHidePrerequisite(string prerequisite, bool instant = false)
        {
            this.hidePrerequisites.Add(prerequisite);
            this.UpdateVisibility(instant);
        }

        public void AddShowPrerequisite(string prerequisite, bool instant = false)
        {
            this.showPrerequisites.Add(prerequisite);
            this.UpdateVisibility(instant);
        }

        public void AttachedToEntity(Entity entity)
        {
            base.GetComponent<CanvasGroup>().alpha = 0f;
            this.RemoveAll();
        }

        private void OnEnable()
        {
            this.UpdateVisibility(true);
        }

        public void RemoveAll()
        {
            this.showPrerequisites.Clear();
            this.hidePrerequisites.Clear();
        }

        public void RemoveHidePrerequisite(string prerequisite, bool instant = false)
        {
            this.hidePrerequisites.Remove(prerequisite);
            this.UpdateVisibility(instant);
        }

        public void RemoveShowPrerequisite(string prerequisite, bool instant = false)
        {
            this.showPrerequisites.Remove(prerequisite);
            this.UpdateVisibility(instant);
        }

        private void UpdateVisibility(bool instant = false)
        {
            if (this.Animator.isActiveAndEnabled)
            {
                this.Animator.SetBool("NoAnimation", instant);
                this.Animator.SetBool("Visible", !this.ShouldBeHidden);
            }
        }

        private UnityEngine.Animator Animator
        {
            get
            {
                if (this.animator == null)
                {
                    this.animator = base.GetComponent<UnityEngine.Animator>();
                }
                return this.animator;
            }
        }

        private bool ShouldBeHidden =>
            (this.showPrerequisites.Count == 0) || (this.hidePrerequisites.Count > 0);
    }
}

