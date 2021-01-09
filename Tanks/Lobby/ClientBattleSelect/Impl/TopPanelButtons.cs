namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using UnityEngine;

    public class TopPanelButtons : MonoBehaviour
    {
        private TopPanelButton[] buttons;
        private int lastActivatedButtonIndex;

        public void ActivateButton(int index)
        {
            if (index < this.Buttons.Length)
            {
                this.ActivateButton(this.Buttons[index]);
            }
        }

        public void ActivateButton(TopPanelButton button)
        {
            foreach (TopPanelButton button2 in this.Buttons)
            {
                button2.Activated = false;
            }
            button.Activated = true;
            int index = Array.IndexOf<TopPanelButton>(this.Buttons, button);
            bool flag = index > this.lastActivatedButtonIndex;
            this.Buttons[this.lastActivatedButtonIndex].ImageFillToRight = !flag;
            this.Buttons[index].ImageFillToRight = flag;
            this.lastActivatedButtonIndex = index;
        }

        private TopPanelButton[] Buttons
        {
            get
            {
                this.buttons ??= base.GetComponentsInChildren<TopPanelButton>(true);
                return this.buttons;
            }
        }
    }
}

