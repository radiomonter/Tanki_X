namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    public class ModuleCardsTierUI : MonoBehaviour
    {
        public void AddCard(ModuleCardItemUIComponent moduleCardItem)
        {
            moduleCardItem.transform.SetParent(base.transform, false);
            this.SortCards();
        }

        public void Clear()
        {
            ModuleCardItemUIComponent[] componentsInChildren = base.GetComponentsInChildren<ModuleCardItemUIComponent>(true);
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                Destroy(componentsInChildren[i].gameObject);
            }
        }

        public ModuleCardItemUIComponent GetCard(long marketItemGroupId)
        {
            foreach (ModuleCardItemUIComponent component in base.GetComponentsInChildren<ModuleCardItemUIComponent>(true))
            {
                if (component.MarketGroupId == marketItemGroupId)
                {
                    return component;
                }
            }
            return null;
        }

        public void SortCards()
        {
            foreach (ModuleCardItemUIComponent component in base.GetComponentsInChildren<ModuleCardItemUIComponent>(false))
            {
                if (component.Type == ModuleBehaviourType.ACTIVE)
                {
                    component.transform.SetAsFirstSibling();
                }
            }
        }
    }
}

