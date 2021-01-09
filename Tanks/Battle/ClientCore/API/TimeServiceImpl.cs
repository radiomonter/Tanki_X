namespace Tanks.Battle.ClientCore.API
{
    using log4net;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    public class TimeServiceImpl : TimeService
    {
        public static float SMOOTH_RATE = 0.1f;
        public static long MAX_DELTA_DIFF_TO_SERVER = 0x1388L;
        private long diffToServer;
        private bool serverTimeInited;
        private long initServerTime;
        private float initUnityRealTime;
        private bool smoothing;
        private float smoothBeginTime;
        private long smoothedDiffToServer;
        private long deltaDiffToServer;
        private long absDeltaDiffToServer;
        private ILog log;

        public TimeServiceImpl()
        {
            this.log = LoggerProvider.GetLogger(this);
        }

        private void CheckInited()
        {
            if (!this.serverTimeInited)
            {
                throw new Exception("Server time not inited");
            }
        }

        public void InitServerTime(long serverTime)
        {
            this.initServerTime = serverTime;
            this.initUnityRealTime = UnityTime.realtimeSinceStartup;
            this.diffToServer = this.initServerTime - ((long) (this.initUnityRealTime * 1000f));
            this.serverTimeInited = true;
            this.log.InfoFormat("InitServerTime: serverTime={0} unityRealTime={1} diffToServer={2}", serverTime, this.initUnityRealTime, this.diffToServer);
        }

        public void SetDiffToServerWithSmoothing(long newDiffToServer)
        {
            this.UpdateSmoothing();
            this.deltaDiffToServer = newDiffToServer - this.smoothedDiffToServer;
            this.absDeltaDiffToServer = Math.Abs(this.deltaDiffToServer);
            if (this.absDeltaDiffToServer > MAX_DELTA_DIFF_TO_SERVER)
            {
                this.log.ErrorFormat("Delta too large: {0}", this.absDeltaDiffToServer);
                this.deltaDiffToServer = (this.deltaDiffToServer <= 0L) ? -MAX_DELTA_DIFF_TO_SERVER : MAX_DELTA_DIFF_TO_SERVER;
                this.absDeltaDiffToServer = MAX_DELTA_DIFF_TO_SERVER;
            }
            this.diffToServer = this.smoothedDiffToServer + this.deltaDiffToServer;
            this.log.InfoFormat("Begin smoothing: deltaDiffToServer={0} wasSmoothing={1}", this.deltaDiffToServer, this.smoothing);
            if (this.deltaDiffToServer != 0L)
            {
                this.smoothing = true;
                this.smoothBeginTime = UnityTime.realtimeSinceStartup;
            }
        }

        public void UpdateSmoothing()
        {
            if (!this.smoothing)
            {
                this.smoothedDiffToServer = this.diffToServer;
            }
            else
            {
                float num = UnityTime.realtimeSinceStartup - this.smoothBeginTime;
                float num2 = (((float) this.absDeltaDiffToServer) / 1000f) / SMOOTH_RATE;
                if (num < num2)
                {
                    long num4 = this.diffToServer - this.deltaDiffToServer;
                    this.smoothedDiffToServer = num4 + ((long) ((num / num2) * this.deltaDiffToServer));
                }
                else
                {
                    this.log.InfoFormat("End smoothing: diffToServer={0}", this.diffToServer);
                    this.smoothedDiffToServer = this.diffToServer;
                    this.smoothing = false;
                }
            }
        }

        [Inject]
        public static Platform.Library.ClientUnityIntegration.API.UnityTime UnityTime { get; set; }

        public long DiffToServer
        {
            get
            {
                this.CheckInited();
                this.UpdateSmoothing();
                return this.smoothedDiffToServer;
            }
            set
            {
                this.CheckInited();
                this.log.InfoFormat("SetDiffToServer: {0}", value);
                this.smoothing = false;
                this.diffToServer = value;
            }
        }
    }
}

