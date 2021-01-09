namespace Tanks.Lobby.ClientControls.API
{
    using System;
    using UnityEngine;

    public interface SpriteRequest
    {
        void Cancel();
        void Resolve(Sprite sprite);

        string Uid { get; }
    }
}

