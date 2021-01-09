namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class ConfirmDialogComponent : BaseDialogComponent
    {
        private List<Animator> animators;
        public Action dialogClosed;
        private bool _show;

        public override void Hide()
        {
            if (this.show)
            {
                MainScreenComponent.Instance.ClearOnBackOverride();
                this.show = false;
                Animator component = base.GetComponent<Animator>();
                if (component != null)
                {
                    component.SetBool("show", false);
                }
                this.ShowHiddenScreenParts();
            }
        }

        public void HideImmediate()
        {
            this.Hide();
            this.OnHide();
        }

        private void OnDisable()
        {
            this.ShowHiddenScreenParts();
        }

        protected virtual void OnEnable()
        {
            CanvasGroup componentInChildren = base.GetComponentInChildren<CanvasGroup>();
            if (componentInChildren != null)
            {
                componentInChildren.alpha = 0f;
            }
            Animator component = base.GetComponent<Animator>();
            if (component != null)
            {
                component.SetBool("show", true);
            }
            if (this.animators != null)
            {
                foreach (Animator animator2 in this.animators)
                {
                    animator2.SetBool("Visible", false);
                }
            }
        }

        public void OnHide()
        {
            if (this.show)
            {
                this.OnEnable();
            }
            else
            {
                if (this.dialogClosed != null)
                {
                    this.dialogClosed();
                }
                base.gameObject.SetActive(false);
            }
        }

        public virtual void OnShow()
        {
        }

        public void OnYes()
        {
            this.Hide();
            if (base.GetComponent<EntityBehaviour>() != null)
            {
                Entity entity = base.GetComponent<EntityBehaviour>().Entity;
                base.ScheduleEvent<DialogConfirmEvent>(entity);
            }
        }

        public override void Show(List<Animator> animtrs = null)
        {
            if (MainScreenComponent.Instance != null)
            {
                MainScreenComponent.Instance.OverrideOnBack(new Action(this.Hide));
            }
            animtrs ??= new List<Animator>();
            this.animators = animtrs;
            this.show = true;
            if (base.gameObject.activeInHierarchy)
            {
                this.OnEnable();
            }
            else
            {
                base.gameObject.SetActive(true);
            }
        }

        protected void ShowHiddenScreenParts()
        {
            if (this.animators != null)
            {
                foreach (Animator animator in this.animators)
                {
                    animator.SetBool("Visible", true);
                }
                this.animators = null;
            }
        }

        public bool show
        {
            get => 
                this._show;
            set => 
                this._show = value;
        }
    }
}

