namespace Tanks.Battle.ClientHUD.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using UnityEngine;
    using UnityEngine.UI;

    public class ServiceMessageComponent : MonoBehaviour, Component
    {
        public Animator animator;
        public Text MessageText;
    }
}

