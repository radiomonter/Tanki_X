namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    public class TabManager : MonoBehaviour
    {
        [SerializeField]
        protected RectTransform tabsContainer;
        public int index;

        protected virtual void OnDisable()
        {
            Tab[] componentsInChildren = base.GetComponentsInChildren<Tab>(true);
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                componentsInChildren[i].Hide();
            }
        }

        protected virtual void OnEnable()
        {
            this.Show(this.index);
        }

        public virtual void Show(int newIndex)
        {
            this.index = newIndex;
            Tab[] componentsInChildren = base.GetComponentsInChildren<Tab>(true);
            componentsInChildren[newIndex].Show();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                if (i != newIndex)
                {
                    componentsInChildren[i].Hide();
                }
            }
        }
    }
}

