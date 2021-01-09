namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientGarage.API;

    public class VisualItemsDropDownList : DropDownListComponent
    {
        protected override void OnItemSelect(ListItem item)
        {
            base.OnItemSelect(item);
            VisualItem data = (VisualItem) item.Data;
            base.listTitle.text = data.Name;
        }

        public void UpdateList(List<VisualItem> items)
        {
            if (items.Count != 0)
            {
                base.dataProvider.ClearItems();
                VisualItem selected = null;
                foreach (VisualItem item2 in items)
                {
                    if (item2.IsSelected)
                    {
                        selected = item2;
                    }
                }
                base.listTitle.text = (selected == null) ? "None" : selected.Name;
                base.dataProvider.Init<VisualItem>(items, selected);
            }
        }
    }
}

