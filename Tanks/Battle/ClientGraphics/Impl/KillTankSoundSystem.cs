namespace Tanks.Battle.ClientGraphics.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;

    public class KillTankSoundSystem : ECSSystem
    {
        [OnEventFire]
        public void PlayKillSound(KillEvent e, SelfBattleUserNode battleUser, [JoinByUser] TankNode tank, [JoinAll] SingleNode<SoundListenerComponent> listener)
        {
            if (KillTankSoundEffectBehaviour.CreateKillTankSound(tank.killTankSoundEffect.EffectPrefab))
            {
                base.ScheduleEvent<KillTankSoundEffectCreatedEvent>(listener);
            }
        }

        public class SelfBattleUserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public UserGroupComponent userGroup;
        }

        public class TankNode : Node
        {
            public KillTankSoundEffectComponent killTankSoundEffect;
            public UserGroupComponent userGroup;
        }
    }
}

