namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public static class UserABTestByID
    {
        public static int GetExperimentId(Entity user) => 
            (int) (user.Id % 2L);
    }
}

