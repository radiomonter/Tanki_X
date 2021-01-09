namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CompactScreenBehaviour : MonoBehaviour
    {
        private State state;
        private Resolution avgRes;
        [CompilerGenerated]
        private static Func<Resolution, int> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<Resolution, int> <>f__am$cache1;

        private void ApplyCompactScreenData()
        {
            Screen.SetResolution(this.avgRes.width, this.avgRes.height, false);
        }

        private void ApplyInitialScreenData()
        {
            GraphicsSettings.INSTANCE.ApplyInitialScreenResolutionData();
        }

        public void DisableCompactMode()
        {
            this.ApplyInitialScreenData();
            if (ApplicationFocusBehaviour.INSTANCE.Focused)
            {
                Destroy(this);
            }
            else
            {
                this.state = State.DESTRUCTION;
            }
        }

        public void InitCompactMode()
        {
            <InitCompactMode>c__AnonStorey0 storey = new <InitCompactMode>c__AnonStorey0();
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = r => r.width;
            }
            storey.avgWidth = Convert.ToInt32(GraphicsSettings.INSTANCE.ScreenResolutions.Average<Resolution>(<>f__am$cache0));
            <>f__am$cache1 ??= r => r.height;
            storey.avgHeight = Convert.ToInt32(GraphicsSettings.INSTANCE.ScreenResolutions.Average<Resolution>(<>f__am$cache1));
            this.avgRes = GraphicsSettings.INSTANCE.ScreenResolutions.OrderBy<Resolution, int>(new Func<Resolution, int>(storey.<>m__0)).First<Resolution>();
            Resolution currentResolution = GraphicsSettings.INSTANCE.CurrentResolution;
            if ((currentResolution.width + currentResolution.height) < (this.avgRes.width + this.avgRes.height))
            {
                this.avgRes = currentResolution;
            }
            this.ApplyCompactScreenData();
            this.state = !ApplicationFocusBehaviour.INSTANCE.Focused ? State.COMPACT : State.IDLE;
        }

        private void OnApplicationQuit()
        {
            GraphicsSettings.INSTANCE.SaveWindowModeOnQuit();
        }

        private void Update()
        {
            if (ApplicationFocusBehaviour.INSTANCE.Focused)
            {
                State state = this.state;
                if (state == State.COMPACT)
                {
                    this.ApplyCompactScreenData();
                    this.state = State.IDLE;
                }
                else if (state == State.DESTRUCTION)
                {
                    this.ApplyInitialScreenData();
                    Destroy(this);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <InitCompactMode>c__AnonStorey0
        {
            internal int avgWidth;
            internal int avgHeight;

            internal int <>m__0(Resolution r) => 
                Mathf.Abs((int) (r.width - this.avgWidth)) + Mathf.Abs((int) (r.height - this.avgHeight));
        }

        private enum State
        {
            IDLE,
            COMPACT,
            DESTRUCTION
        }
    }
}

