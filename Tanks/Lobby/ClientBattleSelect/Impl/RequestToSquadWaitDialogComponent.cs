﻿namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using UnityEngine;
    using UnityEngine.EventSystems;

    [RequireComponent(typeof(WaitDialogComponent))]
    public class RequestToSquadWaitDialogComponent : UIBehaviour, Component
    {
    }
}

