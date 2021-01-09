namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GroupSelector : MonoBehaviour
    {
        private List<SimpleSelectableComponent> items = new List<SimpleSelectableComponent>();

        public void Add(SimpleSelectableComponent item)
        {
            this.items.Add(item);
            item.AddHandler(new Action<SimpleSelectableComponent, bool>(this.Select));
            item.AddDestroyHandler(new Action<SimpleSelectableComponent>(this.Remove));
        }

        public void Remove(SimpleSelectableComponent item)
        {
            this.items.Remove(item);
        }

        public void Select(SimpleSelectableComponent item, bool selected)
        {
            <Select>c__AnonStorey0 storey = new <Select>c__AnonStorey0 {
                item = item
            };
            if (selected)
            {
                this.items.ForEach(new Action<SimpleSelectableComponent>(storey.<>m__0));
            }
        }

        [CompilerGenerated]
        private sealed class <Select>c__AnonStorey0
        {
            internal SimpleSelectableComponent item;

            internal void <>m__0(SimpleSelectableComponent x)
            {
                if ((x != this.item) && ((x != null) && (x.gameObject != null)))
                {
                    x.Select(false);
                }
            }
        }
    }
}

