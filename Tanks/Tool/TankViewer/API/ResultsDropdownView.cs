namespace Tanks.Tool.TankViewer.API
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class ResultsDropdownView : MonoBehaviour
    {
        public ResultsDataSource resultsDataSource;
        public Dropdown dropdown;

        public void SelectLastOption()
        {
            this.dropdown.value = this.dropdown.options.Count - 1;
            this.dropdown.RefreshShownValue();
        }

        public void Start()
        {
            if (this.resultsDataSource.IsReady)
            {
                this.UpdateView();
            }
            this.resultsDataSource.onChange += new Action(this.UpdateView);
        }

        public void UpdateView()
        {
            List<string> folderNames = this.resultsDataSource.GetFolderNames();
            this.dropdown.ClearOptions();
            this.dropdown.options.Add(new Dropdown.OptionData("none"));
            foreach (string str in folderNames)
            {
                this.dropdown.options.Add(new Dropdown.OptionData(str));
            }
            this.dropdown.value = 0;
            this.dropdown.RefreshShownValue();
        }
    }
}

