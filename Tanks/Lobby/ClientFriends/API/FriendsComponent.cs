namespace Tanks.Lobby.ClientFriends.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class FriendsComponent : Component
    {
        public Dictionary<long, DateTime> InLobbyInvitations = new Dictionary<long, DateTime>();
        public Dictionary<long, DateTime> InSquadInvitations = new Dictionary<long, DateTime>();
        [CompilerGenerated]
        private static Func<long, string> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<long, string> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<long, string> <>f__am$cache2;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = id => id.ToString();
            }
            builder.Append("\n[" + string.Join(", ", this.AcceptedFriendsIds.Select<long, string>(<>f__am$cache0).ToArray<string>()) + "]\n");
            <>f__am$cache1 ??= id => id.ToString();
            builder.Append("\n[" + string.Join(", ", this.IncommingFriendsIds.Select<long, string>(<>f__am$cache1).ToArray<string>()) + "]\n");
            <>f__am$cache2 ??= id => id.ToString();
            builder.Append("\n[" + string.Join(", ", this.OutgoingFriendsIds.Select<long, string>(<>f__am$cache2).ToArray<string>()) + "]\n");
            return builder.ToString();
        }

        public HashSet<long> AcceptedFriendsIds { get; set; }

        public HashSet<long> IncommingFriendsIds { get; set; }

        public HashSet<long> OutgoingFriendsIds { get; set; }
    }
}

