namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class PremiumToolbarUiComponent : BaseDialogComponent
    {
        public LocalizedField daysTextLocalizedField;
        public LocalizedField hoursTextLocalizedField;
        public TextMeshProUGUI activeText;
        public TextMeshProUGUI questText;
        public Animator animator;
        public bool visible;

        public void ActivatePremiumTasks()
        {
            this.questText.color = Color.white;
        }

        public void DeactivatePremiumTasks()
        {
            this.questText.color = Color.gray;
        }

        public void Hidden()
        {
            this.visible = false;
        }

        public override void Hide()
        {
            if (this.visible)
            {
                this.animator.SetBool("visible", false);
            }
        }

        public override void Show(List<Animator> animators = null)
        {
            if (!this.visible)
            {
                this.animator.SetBool("visible", true);
            }
        }

        public void Toggle()
        {
            if (this.visible)
            {
                this.Hide();
            }
            else
            {
                this.Show(null);
            }
        }

        public void Visible()
        {
            this.visible = true;
        }
    }
}

