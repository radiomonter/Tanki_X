namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class CurrentAndNextRankExperienceComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private UnityEngine.UI.Text text;

        public UnityEngine.UI.Text Text =>
            this.text;
    }
}

