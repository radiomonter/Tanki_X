using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using System;

public class GoodsPriceSystem : ECSSystem
{
    [OnEventFire]
    public void UpdateGoodsPrice(UpdateGoodsPriceEvent e, GoodsNode goods)
    {
        if (goods.Entity.HasComponent<GoodsPriceComponent>())
        {
            GoodsPriceComponent component = goods.Entity.GetComponent<GoodsPriceComponent>();
            component.Currency = e.Currency;
            component.Price = e.Price;
        }
        else
        {
            GoodsPriceComponent component = new GoodsPriceComponent {
                Currency = e.Currency,
                Price = e.Price
            };
            goods.Entity.AddComponent(component);
        }
        if (goods.Entity.HasComponent<SpecialOfferComponent>())
        {
            goods.Entity.GetComponent<SpecialOfferComponent>().Discount = e.DiscountCoeff * 100f;
        }
        base.ScheduleEvent<GoodsChangedEvent>(goods.Entity);
    }

    public class GoodsNode : Node
    {
        public GoodsComponent goods;
    }
}

