namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;

    public class PersonalChatChannelUIComponent : BehaviourComponent, AttachToEntityListener
    {
        private Entity entity;

        public void AttachedToEntity(Entity entity)
        {
            this.entity = entity;
        }

        public void OnChannelClose()
        {
            if (this.entity != null)
            {
                ECSBehaviour.EngineService.Engine.ScheduleEvent(new CloseChannelEvent(), this.entity);
            }
        }
    }
}

