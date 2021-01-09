namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class CommonScreenElementsComponent : MonoBehaviour, Component, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<string> names;
        [SerializeField]
        private List<Animator> items;
        private Dictionary<string, Animator> itemsMap = new Dictionary<string, Animator>();

        public void ActivateItems(ICollection<string> names)
        {
            foreach (KeyValuePair<string, Animator> pair in this.itemsMap)
            {
                if ((pair.Value != null) && pair.Value.gameObject.activeSelf)
                {
                    pair.Value.SetBool("Visible", false);
                }
            }
            foreach (string str in names)
            {
                if (!this.itemsMap.ContainsKey(str))
                {
                    throw new ArgumentException("TopPanel item with name " + str + " not found!");
                }
                Animator animator = this.itemsMap[str];
                if (animator != null)
                {
                    animator.gameObject.SetActive(true);
                    animator.SetBool("Visible", true);
                }
            }
        }

        public void HideItem(string name)
        {
            this.itemsMap[name].SetBool("Visible", false);
        }

        public void OnAfterDeserialize()
        {
            this.itemsMap.Clear();
            for (int i = 0; i < this.names.Count; i++)
            {
                this.itemsMap.Add(this.names[i], this.items[i]);
            }
        }

        public void OnBeforeSerialize()
        {
            this.names = this.itemsMap.Keys.ToList<string>();
            this.items = this.itemsMap.Values.ToList<Animator>();
        }

        public void ShowItem(string name)
        {
            this.itemsMap[name].SetBool("Visible", true);
        }
    }
}

