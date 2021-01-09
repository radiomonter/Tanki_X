namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class ShowScreenData
    {
        public ShowScreenData(Type screenType, Tanks.Lobby.ClientNavigation.API.AnimationDirection animationDirection = 4)
        {
            this.ScreenType = screenType;
            this.AnimationDirection = animationDirection;
        }

        public void FreeContext()
        {
            if ((this.Context != null) && this.AutoDeleteContext)
            {
                EngineService.Engine.DeleteEntity(this.Context);
            }
        }

        public ShowScreenData InvertAnimationDirection(Tanks.Lobby.ClientNavigation.API.AnimationDirection animationDirection)
        {
            switch (animationDirection)
            {
                case Tanks.Lobby.ClientNavigation.API.AnimationDirection.DOWN:
                    this.AnimationDirection = Tanks.Lobby.ClientNavigation.API.AnimationDirection.UP;
                    break;

                case Tanks.Lobby.ClientNavigation.API.AnimationDirection.UP:
                    this.AnimationDirection = Tanks.Lobby.ClientNavigation.API.AnimationDirection.DOWN;
                    break;

                case Tanks.Lobby.ClientNavigation.API.AnimationDirection.LEFT:
                    this.AnimationDirection = Tanks.Lobby.ClientNavigation.API.AnimationDirection.RIGHT;
                    break;

                case Tanks.Lobby.ClientNavigation.API.AnimationDirection.RIGHT:
                    this.AnimationDirection = Tanks.Lobby.ClientNavigation.API.AnimationDirection.LEFT;
                    break;

                default:
                    break;
            }
            return this;
        }

        public override string ToString()
        {
            object[] objArray1 = new object[5];
            objArray1[0] = this.ScreenType.Name;
            objArray1[1] = " Context: ";
            objArray1[2] = (this.Context == null) ? "null" : this.Context.Id.ToString();
            object[] local1 = objArray1;
            local1[3] = " AutoDeleteContext: ";
            local1[4] = this.AutoDeleteContext;
            return string.Concat(local1);
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        public Type ScreenType { get; set; }

        public Entity Context { get; set; }

        public bool AutoDeleteContext { get; set; }

        public Tanks.Lobby.ClientNavigation.API.AnimationDirection AnimationDirection { get; set; }
    }
}

