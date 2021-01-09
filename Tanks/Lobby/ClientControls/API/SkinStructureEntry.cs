namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    [Serializable]
    public class SkinStructureEntry
    {
        [SerializeField]
        private string name;
        [SerializeField]
        private string uid;
        [SerializeField]
        private string parentUid;

        public string Name
        {
            get => 
                this.name;
            set => 
                this.name = value;
        }

        public string Uid
        {
            get => 
                this.uid;
            set => 
                this.uid = value;
        }

        public string ParentUid
        {
            get => 
                this.parentUid;
            set => 
                this.parentUid = value;
        }
    }
}

