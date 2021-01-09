using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using System;
using System.Runtime.CompilerServices;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

public class GameModesStepValidator : MonoBehaviour, ITutorialShowStepValidator
{
    public bool ShowAllowed(long stepId) => 
        true;

    [Inject]
    public static EngineServiceInternal EngineService { get; set; }
}

