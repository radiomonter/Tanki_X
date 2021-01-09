namespace Platform.Library.ClientUnityIntegration.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using UnityEngine;

    public class ServerConnectionBehaviour : MonoBehaviour
    {
        private const int DISCONNECTED = 0;
        private const int CONNECTED = 1;
        private const int CONNECTION_ERROR = 2;
        private bool isConnecting;
        private volatile int connectionStatus;
        private string host;
        private int[] ports = new int[] { 0x13e2 };
        private int networkSliceTime;
        private int portIndex;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Action CompleteEvent;
        [CompilerGenerated]
        private static Func<int, string> <>f__am$cache0;

        public event Action CompleteEvent
        {
            add
            {
                Action completeEvent = this.CompleteEvent;
                while (true)
                {
                    Action objB = completeEvent;
                    completeEvent = Interlocked.CompareExchange<Action>(ref this.CompleteEvent, objB + value, completeEvent);
                    if (ReferenceEquals(completeEvent, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                Action completeEvent = this.CompleteEvent;
                while (true)
                {
                    Action objB = completeEvent;
                    completeEvent = Interlocked.CompareExchange<Action>(ref this.CompleteEvent, objB - value, completeEvent);
                    if (ReferenceEquals(completeEvent, objB))
                    {
                        return;
                    }
                }
            }
        }

        private void CheckConnectionStatus()
        {
            if (this.connectionStatus == 1)
            {
                this.isConnecting = false;
            }
            else if (this.connectionStatus == 2)
            {
                this.connectionStatus = 0;
                this.portIndex++;
                if (this.portIndex >= this.ports.Length)
                {
                    base.enabled = false;
                    this.SendNoServerConnection();
                }
                else
                {
                    try
                    {
                        this.TryConnect(null);
                    }
                    catch (SocketException)
                    {
                        this.SendNoServerConnection();
                    }
                }
            }
        }

        private void DisconnectIfConnected()
        {
            if ((NetworkService != null) && NetworkService.IsConnected)
            {
                NetworkService.Disconnect();
            }
        }

        public bool IsConnecting() => 
            this.isConnecting;

        public void LateUpdate()
        {
            if (NetworkService.IsConnected)
            {
                Flow.Current.Finish();
                NetworkService.WriteCommands();
                Flow.Current.Clean();
            }
        }

        private static void LogConnectionError(string message, string host, int[] ports)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = p => p.ToString();
            }
            LoggerProvider.GetLogger<ServerConnectionBehaviour>().ErrorFormat("{0}{1}:{2}", message, host, string.Join(",", ports.Select<int, string>(<>f__am$cache0).ToArray<string>()));
        }

        public void OnApplicationQuit()
        {
            this.DisconnectIfConnected();
        }

        public void OnDestroy()
        {
            this.DisconnectIfConnected();
        }

        public void OpenConnection(string host, int[] ports)
        {
            this.OpenConnection(host, ports, 0, 0, null, false);
        }

        public void OpenConnection(string host, int[] ports, int networkSliceTime, int networkMaxDelayTime, Action completeAction, bool splitShareCommand)
        {
            if (NetworkService.IsConnected)
            {
                Debug.LogWarning("Already connected.");
            }
            else
            {
                NetworkService.SplitShareCommand = splitShareCommand;
                this.host = host;
                this.networkSliceTime = networkSliceTime;
                this.NetworkMaxDelayTime = networkMaxDelayTime;
                if (ports.Length > 0)
                {
                    this.ports = ports;
                }
                this.PrefetchSocketPolicyForWebplayer(host);
                this.TryConnect(completeAction);
            }
        }

        private void PrefetchSocketPolicyForWebplayer(string host)
        {
        }

        private void SendNoServerConnection()
        {
            LogConnectionError("Could not connect: ", this.host, this.ports);
            Engine engine = EngineService.Engine;
            engine.ScheduleEvent<NoServerConnectionEvent>(engine.CreateEntity("NoConnection"));
        }

        private void TryConnect(Action completeAction = null)
        {
            this.CompleteEvent += () => (this.connectionStatus = !NetworkService.IsConnected ? 2 : 1);
            if (completeAction != null)
            {
                this.CompleteEvent += completeAction;
            }
            this.isConnecting = true;
            NetworkService.ConnectAsync(this.host, this.ports[this.portIndex], this.CompleteEvent);
        }

        public void Update()
        {
            if (this.isConnecting)
            {
                this.CheckConnectionStatus();
            }
            else if (!NetworkService.IsConnected)
            {
                base.enabled = false;
                Engine engine = EngineService.Engine;
                LogConnectionError("Server disconnected: ", this.host, this.ports);
                engine.ScheduleEvent<ServerDisconnectedEvent>(engine.CreateEntity("ServerDisconnected"));
            }
            else
            {
                try
                {
                    NetworkService.ReadAndExecuteCommands((long) this.networkSliceTime, (long) this.NetworkMaxDelayTime);
                }
                finally
                {
                }
            }
        }

        [Inject]
        public static Platform.System.Data.Exchange.ClientNetwork.API.NetworkService NetworkService { get; set; }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        public int NetworkMaxDelayTime { get; set; }
    }
}

