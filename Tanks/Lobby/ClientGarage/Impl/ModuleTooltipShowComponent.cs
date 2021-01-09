namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class ModuleTooltipShowComponent : TooltipShowBehaviour
    {
        public void ShowTooltip(object data)
        {
            CheckForTutorialEvent eventInstance = new CheckForTutorialEvent();
            TooltipShowBehaviour.EngineService.Engine.ScheduleEvent(eventInstance, TooltipShowBehaviour.EngineService.EntityStub);
            if (!eventInstance.TutorialIsActive)
            {
                TooltipController.Instance.ShowTooltip(Input.mousePosition, data, !base.customContentPrefab ? null : base.customPrefab, true);
            }
        }

        public override void ShowTooltip(Vector3 mousePosition)
        {
            base.tooltipShowed = true;
            Entity moduleEntity = base.GetComponent<ModuleCardItemUIComponent>().ModuleEntity;
            Engine engine = TooltipShowBehaviour.EngineService.Engine;
            CheckForTutorialEvent eventInstance = new CheckForTutorialEvent();
            engine.ScheduleEvent(eventInstance, TooltipShowBehaviour.EngineService.EntityStub);
            if (!eventInstance.TutorialIsActive)
            {
                engine.ScheduleEvent<ModuleTooltipShowEvent>(moduleEntity);
            }
        }
    }
}

