namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Collections.Generic;
    using Tanks.Lobby.ClientControls.API;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;

    public class PresetsDropDownList : DropDownListComponent
    {
        private bool AllowSelectPresetItem(PresetItem item, PresetItem selected)
        {
            if (selected == null)
            {
                return true;
            }
            long key = selected.presetEntity.GetComponent<UserGroupComponent>().Key;
            long id = SelfUserComponent.SelfUser.Id;
            return ((item.presetEntity.GetComponent<UserGroupComponent>().Key == id) ? (key == id) : (SelfUserComponent.SelfUser.HasComponent<UserUseItemsPrototypeComponent>() && (SelfUserComponent.SelfUser.GetComponent<UserUseItemsPrototypeComponent>().Preset.Id == item.presetEntity.Id)));
        }

        protected override void OnItemSelect(ListItem item)
        {
            base.OnItemSelect(item);
            base.listTitle.text = ((PresetItem) item.Data).Name;
        }

        public void UpdateList(List<PresetItem> items)
        {
            base.dataProvider.ClearItems();
            PresetItem selected = null;
            foreach (PresetItem item2 in items)
            {
                if (item2.isSelected && this.AllowSelectPresetItem(item2, selected))
                {
                    selected = item2;
                }
            }
            base.dataProvider.Init<PresetItem>(items, selected);
            base.listTitle.text = selected.Name;
        }
    }
}

