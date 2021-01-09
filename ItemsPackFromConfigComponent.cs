using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[Shared, SerialVersionUID(0x8d424e9e69be67fL)]
public class ItemsPackFromConfigComponent : Component
{
    public List<long> Pack { get; set; }
}

