namespace Tanks.Lobby.ClientControls.API
{
    using System;

    public interface SizeController
    {
        void RegisterSpriteRequest(SpriteRequest request);
        void UnregisterSpriteRequest(SpriteRequest request);
    }
}

