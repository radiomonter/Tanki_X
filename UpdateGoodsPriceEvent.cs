using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using System;
using System.Runtime.CompilerServices;

[Shared, SerialVersionUID(0x179f6bb52b6L)]
public class UpdateGoodsPriceEvent : Event
{
    public string Currency { get; set; }

    public double Price { get; set; }

    public float DiscountCoeff { get; set; }
}

