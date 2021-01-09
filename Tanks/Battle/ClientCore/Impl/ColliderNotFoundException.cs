namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using System.Runtime.CompilerServices;

    public class ColliderNotFoundException : Exception
    {
        public ColliderNotFoundException(TankCollidersUnityComponent tankColliders, string colliderName) : base(string.Concat(objArray1))
        {
            object[] objArray1 = new object[] { "TankColliders=", tankColliders, " colliderName=", colliderName };
            this.TankColliders = tankColliders;
            this.ColliderName = colliderName;
        }

        public TankCollidersUnityComponent TankColliders { get; set; }

        public string ColliderName { get; set; }
    }
}

