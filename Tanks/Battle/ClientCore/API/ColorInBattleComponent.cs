namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ColorInBattleComponent : Component
    {
        public ColorInBattleComponent(Tanks.Battle.ClientCore.API.TeamColor color)
        {
            this.TeamColor = color;
        }

        public Tanks.Battle.ClientCore.API.TeamColor TeamColor { get; set; }
    }
}

