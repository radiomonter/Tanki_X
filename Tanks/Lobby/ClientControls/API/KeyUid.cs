namespace Tanks.Lobby.ClientControls.API
{
    using System;

    public class KeyUid : IComparable<KeyUid>
    {
        public string key;
        public string uid;

        public int CompareTo(KeyUid other) => 
            string.Compare(this.key, other.key, StringComparison.Ordinal);

        public override bool Equals(object obj) => 
            !ReferenceEquals(null, obj) ? ((obj is KeyUid) && this.Equals((KeyUid) obj)) : false;

        public bool Equals(KeyUid other) => 
            string.Equals(this.key, other.key) && string.Equals(this.uid, other.uid);

        public override int GetHashCode() => 
            (((this.key == null) ? 0 : this.key.GetHashCode()) * 0x18d) ^ ((this.uid == null) ? 0 : this.uid.GetHashCode());
    }
}

