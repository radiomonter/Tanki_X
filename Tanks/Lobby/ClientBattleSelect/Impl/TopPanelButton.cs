namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class TopPanelButton : MonoBehaviour
    {
        [SerializeField]
        private Image filledImage;
        private bool activated;

        private void OnDisable()
        {
            this.Activated = false;
        }

        private void OnEnable()
        {
            this.Activated = this.activated;
        }

        public bool ImageFillToRight
        {
            set => 
                this.filledImage.fillOrigin = !value ? 1 : 0;
        }

        public bool Activated
        {
            set
            {
                this.activated = value;
                base.GetComponent<Animator>().SetBool("activated", this.activated);
            }
        }
    }
}

