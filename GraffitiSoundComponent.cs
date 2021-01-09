using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;

public class GraffitiSoundComponent : MonoBehaviour, Component
{
    [SerializeField]
    private AudioSource sound;

    public AudioSource Sound =>
        this.sound;
}

