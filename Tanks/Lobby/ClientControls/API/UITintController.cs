namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [ExecuteInEditMode]
    public class UITintController : MonoBehaviour
    {
        public Color tint = Color.white;
        private Color lastTint;
        private List<UITint> elements = new List<UITint>();

        public void AddElement(UITint element)
        {
            this.elements.Add(element);
        }

        public void RemoveElement(UITint element)
        {
            this.elements.Remove(element);
        }

        protected virtual void SetTint(UITint uiTint, Color tint)
        {
            uiTint.SetTint(tint);
        }

        private void Update()
        {
            if (this.tint != this.lastTint)
            {
                int num = 0;
                while (true)
                {
                    if (num >= this.elements.Count)
                    {
                        this.lastTint = this.tint;
                        break;
                    }
                    this.SetTint(this.elements[num], this.tint);
                    num++;
                }
            }
        }
    }
}

