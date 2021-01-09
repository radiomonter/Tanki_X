namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(ScrollRect))]
    public class BattleResultsScoreTableComponent : BehaviourComponent
    {
        public PlayerStatInfoUI rowPrefab;

        private void OnDisable()
        {
            ScrollRect component = base.GetComponent<ScrollRect>();
            if (component.content != null)
            {
                component.content.DestroyChildren();
            }
        }
    }
}

