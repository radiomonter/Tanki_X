namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using TMPro;
    using UnityEngine;

    public class UidHighlightingComponent : MonoBehaviour, Component
    {
        public FontStyles friend;
        public FontStyles selfUser;
        public FontStyles normal;
    }
}

