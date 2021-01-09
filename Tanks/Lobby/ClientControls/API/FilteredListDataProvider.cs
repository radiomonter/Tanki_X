namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using System.Collections.Generic;

    public class FilteredListDataProvider : DefaultListDataProvider
    {
        private List<object> filteredData;

        public void ApplyFilter(Func<object, bool> IsFiltered)
        {
            this.filteredData ??= new List<object>();
            this.filteredData.Clear();
            foreach (object obj2 in base.dataStorage)
            {
                if (!IsFiltered(obj2))
                {
                    this.filteredData.Add(obj2);
                }
            }
            base.SendChanged();
        }

        public override void ClearItems()
        {
            base.ClearItems();
            if (this.filteredData != null)
            {
                this.filteredData.Clear();
                this.filteredData = null;
            }
        }

        public override IList<object> Data =>
            (this.filteredData != null) ? this.filteredData : base.dataStorage;
    }
}

