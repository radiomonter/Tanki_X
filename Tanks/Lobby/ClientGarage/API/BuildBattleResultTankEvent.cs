namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BuildBattleResultTankEvent : Event
    {
        public string HullGuid { get; set; }

        public string WeaponGuid { get; set; }

        public string PaintGuid { get; set; }

        public string CoverGuid { get; set; }

        public bool BestPlayerScreen { get; set; }

        public RenderTexture tankPreviewRenderTexture { get; set; }
    }
}

