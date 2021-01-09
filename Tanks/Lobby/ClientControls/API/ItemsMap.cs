namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public class ItemsMap : IEnumerable<ListItem>, IEnumerable
    {
        private List<ListItem> items = new List<ListItem>();
        private Dictionary<object, ListItem> map = new Dictionary<object, ListItem>();

        public void Add(ListItem item)
        {
            this.items.Add(item);
            this.map.Add(item.Data, item);
        }

        public void Clear()
        {
            this.items.Clear();
            this.map.Clear();
        }

        public bool Contains(object entity) => 
            this.map.ContainsKey(entity);

        public IEnumerator<ListItem> GetEnumerator() => 
            this.items.GetEnumerator();

        public bool Remove(object entity)
        {
            if (this.map.ContainsKey(entity))
            {
                ListItem item = this.map[entity];
                if (item != null)
                {
                    this.items.Remove(item);
                }
            }
            return this.map.Remove(entity);
        }

        public void Sort(IComparer<ListItem> comparer)
        {
            this.items.Sort(comparer);
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            this.items.GetEnumerator();

        public int Count =>
            this.items.Count;

        public ListItem this[object entity] =>
            this.map[entity];
    }
}

