namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class DefaultDropDownList : DropDownListComponent
    {
        protected override void OnItemSelect(ListItem item)
        {
            base.OnItemSelect(item);
            base.listTitle.text = item.Data as string;
        }

        public void UpdateList(List<string> items, int index = 0)
        {
            base.dataProvider.ClearItems();
            string selected = items[index];
            base.dataProvider.Init<string>(items, selected);
            base.listTitle.text = selected;
        }

        public DefaultListDataProvider DataProvider =>
            base.dataProvider;
    }
}

