namespace Tanks.Lobby.ClientNavigation.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientNavigation.API;

    public class SceneLoaderSystem : ECSSystem
    {
        [OnEventFire]
        public void LoadScene(LoadSceneEvent e, Node newSceneNode, [JoinAll] Optional<SingleNode<CurrentSceneComponent>> currentScene)
        {
            if (currentScene.IsPresent())
            {
                currentScene.Get().Entity.RemoveComponent<CurrentSceneComponent>();
            }
            UnityUtil.LoadScene(e.SceneAsset, e.SceneName, false);
            newSceneNode.Entity.AddComponent<CurrentSceneComponent>();
        }

        [OnEventComplete]
        public void SwitchToEntrance(SwitchToEntranceSceneEvent e, Node node)
        {
            SceneSwitcher.CleanAndRestart();
        }
    }
}

