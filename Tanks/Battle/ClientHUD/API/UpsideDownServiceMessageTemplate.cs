﻿namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;

    [SerialVersionUID(0x14ff9b1b25bL)]
    public interface UpsideDownServiceMessageTemplate : Template
    {
        [AutoAdded, PersistentConfig("", false)]
        UpsideDownMessageComponent upsideDownMessage();
    }
}

