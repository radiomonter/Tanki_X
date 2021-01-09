namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using UnityEngine;

    [SerialVersionUID(0x8d5213de0945014L)]
    public class BattleResultsTankPositionComponent : MonoBehaviour, Component
    {
        public string hullGuid;
        public string weaponGuid;
        public string paintGuid;
        public string coverGuid;
    }
}

