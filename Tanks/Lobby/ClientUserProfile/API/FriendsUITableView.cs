namespace Tanks.Lobby.ClientUserProfile.API
{
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;

    public class FriendsUITableView : UITableView
    {
        private List<UserCellData> items;
        private List<UserCellData> filteredItems;
        private string filterString = string.Empty;

        protected override UITableViewCell CellForRowAtIndex(int index)
        {
            UITableViewCell cell = base.CellForRowAtIndex(index);
            if (cell != null)
            {
                ((FriendsUITableViewCell) cell).Init(this.FilteredItems[index].id, this.Items.Count > 50);
            }
            return cell;
        }

        protected override int NumberOfRows() => 
            this.FilteredItems.Count;

        protected override void OnDisable()
        {
            base.OnDisable();
            this.Items.Clear();
            this.FilterString = string.Empty;
        }

        public void RemoveUser(long userId, bool toRight)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].id == userId)
                {
                    UserCellData item = this.Items[i];
                    this.Items.Remove(item);
                    if (this.FilteredItems.Contains(item))
                    {
                        int index = this.FilteredItems.IndexOf(item);
                        this.FilteredItems.RemoveAt(index);
                        base.RemoveCell(index, toRight);
                    }
                }
            }
        }

        public List<UserCellData> Items
        {
            get
            {
                List<UserCellData> items = this.items;
                if (this.items == null)
                {
                    List<UserCellData> local1 = this.items;
                    items = this.items = new List<UserCellData>();
                }
                return items;
            }
            set => 
                this.items = value;
        }

        public List<UserCellData> FilteredItems
        {
            get
            {
                List<UserCellData> filteredItems = this.filteredItems;
                if (this.filteredItems == null)
                {
                    List<UserCellData> local1 = this.filteredItems;
                    filteredItems = this.filteredItems = new List<UserCellData>();
                }
                return filteredItems;
            }
            set => 
                this.filteredItems = value;
        }

        public string FilterString
        {
            get => 
                this.filterString;
            set
            {
                this.filterString = value;
                this.FilteredItems = new List<UserCellData>();
                foreach (UserCellData data in this.Items)
                {
                    if (string.IsNullOrEmpty(value) || data.uid.ToLower().Contains(this.filterString.ToLower()))
                    {
                        this.FilteredItems.Add(data);
                    }
                }
                base.UpdateTable();
            }
        }
    }
}

