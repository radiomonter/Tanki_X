namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Tanks.Lobby.ClientControls.API.List;
    using UnityEngine;

    public class DefaultListDataProvider : MonoBehaviour, ListDataProvider, IUIList
    {
        [SerializeField]
        private bool clearOnDisable = true;
        protected readonly List<object> dataStorage = new List<object>();
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Action<ListDataProvider> DataChanged;
        private object selected;

        public event Action<ListDataProvider> DataChanged
        {
            add
            {
                Action<ListDataProvider> dataChanged = this.DataChanged;
                while (true)
                {
                    Action<ListDataProvider> objB = dataChanged;
                    dataChanged = Interlocked.CompareExchange<Action<ListDataProvider>>(ref this.DataChanged, objB + value, dataChanged);
                    if (ReferenceEquals(dataChanged, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                Action<ListDataProvider> dataChanged = this.DataChanged;
                while (true)
                {
                    Action<ListDataProvider> objB = dataChanged;
                    dataChanged = Interlocked.CompareExchange<Action<ListDataProvider>>(ref this.DataChanged, objB - value, dataChanged);
                    if (ReferenceEquals(dataChanged, objB))
                    {
                        return;
                    }
                }
            }
        }

        public virtual void AddItem(object data)
        {
            this.dataStorage.Add(data);
            this.SendChanged();
        }

        public virtual void ClearItems()
        {
            this.selected = null;
            this.dataStorage.Clear();
            this.SendChanged();
        }

        public void Init<T>(ICollection<T> data)
        {
            foreach (T local in data)
            {
                this.dataStorage.Add(local);
            }
            this.SendChanged();
        }

        public void Init<T>(ICollection<T> data, T selected)
        {
            foreach (T local in data)
            {
                this.dataStorage.Add(local);
            }
            this.selected = selected;
            this.SendChanged();
        }

        protected virtual void OnDisable()
        {
            if (this.clearOnDisable)
            {
                this.ClearItems();
            }
        }

        private void OnItemSelect(ListItem listItem)
        {
            this.selected = listItem.Data;
        }

        public virtual void RemoveItem(object data)
        {
            this.dataStorage.Remove(data);
            this.SendChanged();
        }

        public void SelectNext()
        {
            if (this.dataStorage.Count != 0)
            {
                int index = this.dataStorage.IndexOf(this.selected);
                this.Selected = (index < 0) ? this.dataStorage[0] : this.dataStorage[Mathf.Min((int) (index + 1), (int) (this.dataStorage.Count - 1))];
            }
        }

        public void SelectPrev()
        {
            if (this.dataStorage.Count != 0)
            {
                int index = this.dataStorage.IndexOf(this.selected);
                this.Selected = (index < 0) ? this.dataStorage[0] : this.dataStorage[Mathf.Max(index - 1, 0)];
            }
        }

        public void SendChanged()
        {
            if (this.DataChanged != null)
            {
                this.DataChanged(this);
            }
        }

        public virtual IList<object> Data =>
            this.dataStorage;

        public object Selected
        {
            get => 
                this.selected;
            set
            {
                if (this.selected != value)
                {
                    this.selected = value;
                    this.SendChanged();
                }
            }
        }
    }
}

