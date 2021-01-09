namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Text;
    using Tanks.Lobby.ClientControls.API;
    using tanks.modules.lobby.ClientControls.Scripts.API;
    using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
    using TMPro;
    using UnityEngine;

    public class NewModuleTooltipContent : MonoBehaviour, ITooltipContent
    {
        public ComplexFillProgressBar progressBar;
        public TextMeshProUGUI nameAndLevel;
        public TextMeshProUGUI definition;
        public LocalizedField levelPrefix;
        public TextMeshProUGUI blueprints;
        private static StringBuilder stringBuilder = new StringBuilder(100);

        public void Init(object data)
        {
            if (!(data is ModuleItem))
            {
                throw new ArgumentException("Incorrect data type " + data.GetType() + ". ModuleItem expected.");
            }
            ModuleItem item = data as ModuleItem;
            stringBuilder.Length = 0;
            stringBuilder.AppendFormat("{0} ({1}{2})", item.Name, this.levelPrefix.Value, item.Level + 1L);
            this.nameAndLevel.text = stringBuilder.ToString();
            this.definition.text = item.Description();
            this.progressBar.ProgressValue = 0f;
            long userCardCount = item.UserCardCount;
            int num2 = (item.UserItem != null) ? item.UpgradePrice : item.CraftPrice.Cards;
            this.blueprints.text = $"{userCardCount}/{num2}";
            this.progressBar.ProgressValue = Mathf.Clamp01(((float) userCardCount) / ((float) num2));
        }
    }
}

