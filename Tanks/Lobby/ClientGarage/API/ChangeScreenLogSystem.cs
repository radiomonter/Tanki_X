namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Text.RegularExpressions;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class ChangeScreenLogSystem : ECSSystem
    {
        public static string lastScreenName = string.Empty;
        public static DateTime lastScreenEnterDateTime = DateTime.Now;

        [OnEventFire]
        public void SendChangeScreenEvent(ChangeScreenLogEvent e, Node any, [JoinAll] SelfUserNode user)
        {
            string nextScreen = this.SplitCamelCase(e.NextScreen.ToString());
            if (lastScreenName != nextScreen)
            {
                double duration = 0.0;
                if (!string.IsNullOrEmpty(lastScreenName))
                {
                    TimeSpan span = DateTime.Now.Subtract(lastScreenEnterDateTime);
                    duration = Math.Truncate((double) ((span.TotalSeconds + (((float) span.Milliseconds) / 1000f)) * 100.0)) / 100.0;
                }
                ChangeScreenEvent eventInstance = new ChangeScreenEvent(lastScreenName, nextScreen, duration);
                lastScreenName = nextScreen;
                lastScreenEnterDateTime = DateTime.Now;
                base.ScheduleEvent(eventInstance, user);
            }
        }

        private string SplitCamelCase(string str) => 
            Regex.Replace(str, "([A-Z])", " $1", RegexOptions.Compiled).Trim();

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
        }
    }
}

