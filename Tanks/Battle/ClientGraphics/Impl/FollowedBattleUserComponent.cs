namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class FollowedBattleUserComponent : Component
    {
        public bool UserHasLeftBattle { get; set; }
    }
}

