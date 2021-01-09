using Platform.Kernel.ECS.ClientEntitySystem.API;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CreateGraffitiEvent : Event
{
    public CreateGraffitiEvent(Vector3 position, Vector3 direction, Vector3 up)
    {
        this.Position = position;
        this.Direction = direction;
        this.Up = up;
    }

    public Vector3 Position { get; set; }

    public Vector3 Direction { get; set; }

    public Vector3 Up { get; set; }

    public bool Success { get; set; }
}

