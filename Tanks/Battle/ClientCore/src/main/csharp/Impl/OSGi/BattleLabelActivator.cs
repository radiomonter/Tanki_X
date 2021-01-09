namespace Tanks.Battle.ClientCore.src.main.csharp.Impl.OSGi
{
    using Platform.Library.ClientUnityIntegration;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class BattleLabelActivator : UnityAwareActivator<AutoCompleting>
    {
        [SerializeField]
        public GameObject BattleLabel;

        protected override void Activate()
        {
            if (this.BattleLabel == null)
            {
                Debug.LogError("BattleLabelActivator: not set prefab UserLabel");
            }
            else
            {
                BattleLabelBuilder.battleLabelPrefab = this.BattleLabel;
            }
        }
    }
}

