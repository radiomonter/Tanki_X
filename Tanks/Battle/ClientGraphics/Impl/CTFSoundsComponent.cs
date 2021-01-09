namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [SerialVersionUID(0x8d2e6e11d8aecfaL)]
    public class CTFSoundsComponent : Component
    {
        public GameObject EffectRoot { get; set; }

        public AudioSource FlagLost { get; set; }

        public AudioSource FlagReturn { get; set; }

        public AudioSource FlagStole { get; set; }

        public AudioSource FlagWin { get; set; }
    }
}

