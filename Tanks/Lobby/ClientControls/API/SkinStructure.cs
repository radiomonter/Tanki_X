namespace Tanks.Lobby.ClientControls.API
{
    using System.Collections.Generic;
    using UnityEngine;

    public class SkinStructure : ScriptableObject
    {
        [SerializeField]
        private List<SkinStructureEntry> categories = new List<SkinStructureEntry>();
        [SerializeField]
        private List<SkinStructureEntry> sprites = new List<SkinStructureEntry>();

        public List<SkinStructureEntry> Categories =>
            this.categories;

        public List<SkinStructureEntry> Sprites =>
            this.sprites;
    }
}

