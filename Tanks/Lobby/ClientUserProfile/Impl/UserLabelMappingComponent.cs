namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine.EventSystems;

    public class UserLabelMappingComponent : UserLabelBaseMappingComponent, IPointerClickHandler, IEventSystemHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            if (base.entity != null)
            {
                ECSBehaviour.EngineService.Engine.ScheduleEvent<UserLabelClickEvent>(base.entity);
            }
        }
    }
}

