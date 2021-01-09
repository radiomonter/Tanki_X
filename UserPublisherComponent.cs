using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using System;
using System.Runtime.CompilerServices;
using Tanks.Lobby.ClientUserProfile.API;

[Shared, SerialVersionUID(0x1d4806b18e61L)]
public class UserPublisherComponent : Component
{
    public Tanks.Lobby.ClientUserProfile.API.Publisher Publisher { get; set; }
}

