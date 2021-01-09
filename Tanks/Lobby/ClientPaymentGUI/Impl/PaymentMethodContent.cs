namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Lobby.ClientPayment.Impl;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using TMPro;
    using UnityEngine;

    public class PaymentMethodContent : MonoBehaviour, ListItemContent
    {
        private Entity entity;
        [SerializeField]
        private ImageListSkin skin;
        [SerializeField]
        private TextMeshProUGUI text;
        [SerializeField]
        private GameObject saleItem;
        [SerializeField]
        private GameObject saleItemLabelEmpty;
        [SerializeField]
        private GameObject saleItemXtraLabelEmpty;
        [SerializeField]
        private TextMeshProUGUI saleItemLabelText;

        private void FillFromEntity(Entity entity)
        {
            if (entity.HasComponent<PaymentMethodComponent>())
            {
                PaymentMethodComponent component = entity.GetComponent<PaymentMethodComponent>();
                this.SetMethodName(component.MethodName);
                this.text.text = component.ShownName;
                this.saleItem.SetActive(false);
                this.saleItemLabelEmpty.SetActive(false);
                this.saleItemXtraLabelEmpty.SetActive(false);
                this.saleItemLabelText.text = string.Empty;
            }
        }

        public void Select()
        {
            if (!this.entity.HasComponent<SelectedListItemComponent>())
            {
                this.entity.AddComponent<SelectedListItemComponent>();
            }
            EngineService.Engine.ScheduleEvent<ListItemSelectedEvent>(this.entity);
        }

        public void SetDataProvider(object dataProvider)
        {
            this.entity = (Entity) dataProvider;
            this.FillFromEntity(this.entity);
        }

        private void SetMethodName(string name)
        {
            this.skin.SelectSprite(name);
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

