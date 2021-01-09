namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [SerialVersionUID(0x8d35d776a515962L)]
    public class ShotIdComponent : Component
    {
        private int shotId;

        public int NextShotId()
        {
            int num;
            this.shotId = num = this.shotId + 1;
            return num;
        }

        public int ShotId =>
            this.shotId;
    }
}

