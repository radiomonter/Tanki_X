namespace Platform.Library.ClientUnityIntegration.API
{
    using log4net;
    using Platform.Library.ClientLogger.API;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class WWWLoader : Loader, IDisposable
    {
        public static int DEFAULT_MAX_ATTEMPTS = 2;
        public static float DEFAULT_TIMEOUT_SECONDS = 30f;
        private UnityEngine.WWW www;
        private int restartAttempts = DEFAULT_MAX_ATTEMPTS;
        private float timeoutSeconds = DEFAULT_TIMEOUT_SECONDS;
        private float lastProgress;
        private float lastProgressTime = Time.time;
        private string errorMessage = string.Empty;
        private static LinkedList<WWWLoader> activeLoaders = new LinkedList<WWWLoader>();
        private ILog Log;

        public WWWLoader(UnityEngine.WWW www)
        {
            this.www = www;
            this.Log = LoggerProvider.GetLogger(this);
            this.Log.InfoFormat("Loading {0}", www.url);
            activeLoaders.AddLast(this);
        }

        public void Dispose()
        {
            this.www.Dispose();
            activeLoaders.Remove(this);
        }

        public static void DisposeActiveLoaders()
        {
            while (activeLoaders.Count > 0)
            {
                activeLoaders.First.Value.Dispose();
            }
        }

        public static int GetResponseCode(UnityEngine.WWW request)
        {
            int result = 0;
            if (request.isDone && ((request.responseHeaders != null) && request.responseHeaders.ContainsKey("STATUS")))
            {
                char[] separator = new char[] { ' ' };
                string[] strArray = request.responseHeaders["STATUS"].Split(separator);
                if (strArray.Length >= 3)
                {
                    int.TryParse(strArray[1], out result);
                }
            }
            return result;
        }

        public void RestartLoad()
        {
            this.restartAttempts--;
            string url = this.www.url;
            this.Log.InfoFormat("RestartLoad URL: {0} restartAttempts: {1}", url, DEFAULT_MAX_ATTEMPTS - this.restartAttempts);
            this.www.Dispose();
            this.www = new UnityEngine.WWW(url);
            this.lastProgress = 0f;
            this.lastProgressTime = Time.time;
            this.errorMessage = string.Empty;
        }

        public override string ToString() => 
            $"[WWWLoader URL={this.URL}]";

        public int MaxRestartAttempts
        {
            get => 
                this.restartAttempts;
            set => 
                this.restartAttempts = value;
        }

        public float TimeoutSeconds
        {
            get => 
                this.timeoutSeconds;
            set => 
                this.timeoutSeconds = value;
        }

        public byte[] Bytes =>
            this.www.bytes;

        public float Progress =>
            this.www.progress;

        public UnityEngine.WWW WWW =>
            this.www;

        public bool IsDone
        {
            get
            {
                if (this.www.isDone)
                {
                    if (string.IsNullOrEmpty(this.www.error) || (this.restartAttempts <= 0))
                    {
                        return true;
                    }
                    this.RestartLoad();
                    return false;
                }
                float time = Time.time;
                if (Math.Abs((float) (this.www.progress - this.lastProgress)) > float.Epsilon)
                {
                    this.lastProgress = this.www.progress;
                    this.lastProgressTime = time;
                    return false;
                }
                if ((time - this.lastProgressTime) <= this.timeoutSeconds)
                {
                    return false;
                }
                if (this.restartAttempts > 0)
                {
                    this.RestartLoad();
                    return false;
                }
                this.Log.InfoFormat("Fail URL: {0}", this.URL);
                this.errorMessage = "Pause of loading was too long";
                return true;
            }
        }

        public string URL =>
            this.www.url;

        public string Error =>
            !string.IsNullOrEmpty(this.www.error) ? this.www.error : this.errorMessage;
    }
}

