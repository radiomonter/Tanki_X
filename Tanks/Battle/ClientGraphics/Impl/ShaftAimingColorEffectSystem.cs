namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class ShaftAimingColorEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void DefineColorForDM(NodeAddedEvent evt, ShaftAimingColorEffectNode weaponNode, [Context, JoinByBattle] DMBattleNode dm)
        {
            weaponNode.shaftAimingColorEffect.ChoosenColor = weaponNode.shaftAimingColorEffect.RedColor;
            weaponNode.Entity.AddComponent<ShaftAimingColorEffectPreparedComponent>();
        }

        [OnEventFire]
        public void DefineColorForTeamMode(NodeAddedEvent evt, [Combine] ShaftAimingTeamColorEffectNode weaponNode, [Context, JoinByTeam] TeamNode team)
        {
            ShaftAimingColorEffectComponent shaftAimingColorEffect = weaponNode.shaftAimingColorEffect;
            shaftAimingColorEffect.ChoosenColor = (team.colorInBattle.TeamColor != TeamColor.BLUE) ? shaftAimingColorEffect.RedColor : shaftAimingColorEffect.BlueColor;
            weaponNode.Entity.AddComponent<ShaftAimingColorEffectPreparedComponent>();
        }

        public class DMBattleNode : Node
        {
            public BattleGroupComponent battleGroup;
            public DMComponent dm;
        }

        public class ShaftAimingColorEffectNode : Node
        {
            public ShaftAimingColorEffectComponent shaftAimingColorEffect;
            public BattleGroupComponent battleGroup;
        }

        public class ShaftAimingTeamColorEffectNode : Node
        {
            public ShaftAimingColorEffectComponent shaftAimingColorEffect;
            public BattleGroupComponent battleGroup;
            public TeamGroupComponent teamGroup;
        }

        public class TeamNode : Node
        {
            public ColorInBattleComponent colorInBattle;
            public TeamGroupComponent teamGroup;
        }
    }
}

