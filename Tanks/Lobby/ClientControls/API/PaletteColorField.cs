namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    [Serializable]
    public class PaletteColorField
    {
        [SerializeField]
        private Palette palette;
        [SerializeField]
        private int uid;

        public UnityEngine.Color Apply(UnityEngine.Color color) => 
            this.palette.Apply(this.uid, color);

        public static implicit operator UnityEngine.Color(PaletteColorField field) => 
            field.Apply(UnityEngine.Color.white);

        public UnityEngine.Color Color =>
            this.Apply(UnityEngine.Color.white);
    }
}

