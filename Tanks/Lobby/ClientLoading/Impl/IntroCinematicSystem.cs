namespace Tanks.Lobby.ClientLoading.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class IntroCinematicSystem : ECSSystem
    {
        [OnEventFire]
        public void Play(NodeAddedEvent e, SingleNode<IntroCinematicComponent> cinematic, SingleNode<LobbyLoadScreenComponent> _)
        {
            if (PlayerPrefs.GetInt("Intro", 0) != 0)
            {
                cinematic.component.OnIntroHide();
            }
            else
            {
                cinematic.Entity.AddComponent<TanyaSleepComponent>();
                PlayerPrefs.SetInt("Intro", 1);
                cinematic.component.Play();
            }
        }
    }
}

