namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using log4net;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientProtocol.Impl;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class NetworkServiceImpl : NetworkService
    {
        private const int BUFFER_SIZE = 0xc800;
        private byte[] readBuffer = new byte[0xc800];
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Action<Command, Exception> OnCommandExecuteException;
        private readonly PlatformSocket socket;
        private readonly Queue<CommandPacket> packetQueue = new Queue<CommandPacket>();
        private readonly ProtocolAdapter protocolAdapter;
        private readonly ILog log;
        private bool infoEnabled;
        private CommandPacket delayedCommands;
        private long delayUntilTime;
        private static readonly DateTime Jan1st1970 = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public event Action<Command, Exception> OnCommandExecuteException
        {
            add
            {
                Action<Command, Exception> onCommandExecuteException = this.OnCommandExecuteException;
                while (true)
                {
                    Action<Command, Exception> objB = onCommandExecuteException;
                    onCommandExecuteException = Interlocked.CompareExchange<Action<Command, Exception>>(ref this.OnCommandExecuteException, objB + value, onCommandExecuteException);
                    if (ReferenceEquals(onCommandExecuteException, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                Action<Command, Exception> onCommandExecuteException = this.OnCommandExecuteException;
                while (true)
                {
                    Action<Command, Exception> objB = onCommandExecuteException;
                    onCommandExecuteException = Interlocked.CompareExchange<Action<Command, Exception>>(ref this.OnCommandExecuteException, objB - value, onCommandExecuteException);
                    if (ReferenceEquals(onCommandExecuteException, objB))
                    {
                        return;
                    }
                }
            }
        }

        public NetworkServiceImpl(ProtocolAdapter protocolAdapter, PlatformSocket socket)
        {
            this.log = LoggerProvider.GetLogger(this);
            this.protocolAdapter = protocolAdapter;
            this.socket = socket;
        }

        public void ConnectAsync(string host, int port, Action completeCallback)
        {
            this.socket.ConnectAsync(host, port, completeCallback);
        }

        private static long CurrentTimeMillis() => 
            (long) (DateTime.UtcNow - Jan1st1970).TotalMilliseconds;

        public void Disconnect()
        {
            this.Disconnect(ProblemStatus.ClosedByClient);
        }

        protected void Disconnect(ProblemStatus status)
        {
            this.log.InfoFormat("Disconnect by {0}.", status);
            if (!this.IsConnected)
            {
                this.log.Warn("Closing not connected socket.");
            }
            else
            {
                try
                {
                    this.socket.Disconnect();
                    this.log.Debug("DISCONNECTED");
                }
                catch (ObjectDisposedException exception)
                {
                    this.log.Fatal("Disconnect error.", exception);
                }
            }
        }

        private void DisconnectOnProblemIfNeeded(ProblemStatus status)
        {
            if ((status == ProblemStatus.SocketMethodInvokeError) || ((status == ProblemStatus.SendError) || (status == ProblemStatus.ReceiveError)))
            {
                this.Disconnect(status);
            }
        }

        private bool ExecuteCommands(CommandPacket packet, long timeToStop, out CommandPacket newPacket)
        {
            int num = 0;
            int count = packet.Commands.Count;
            while (num < count)
            {
                if (CurrentTimeMillis() >= timeToStop)
                {
                    if (this.infoEnabled)
                    {
                        this.log.InfoFormat("Delay execute {0} commands", count - num);
                    }
                    List<Command> commandCollection = ClientNetworkInstancesCache.GetCommandCollection();
                    newPacket = ClientNetworkInstancesCache.GetCommandPacketInstance(commandCollection);
                    for (int i = num; i < count; i++)
                    {
                        commandCollection.Add(packet.Commands[i]);
                    }
                    return false;
                }
                Command command = packet.Commands[num];
                try
                {
                    if (this.infoEnabled)
                    {
                        this.log.InfoFormat("Execute {0}", command);
                    }
                    command.Execute(EngineService.Engine);
                }
                catch (Exception exception)
                {
                    if (this.OnCommandExecuteException != null)
                    {
                        this.OnCommandExecuteException(command, exception);
                    }
                    if (!this.SkipThrowOnCommandExecuteException)
                    {
                        this.OnSocketProblem(ProblemStatus.ExecuteCommandError, exception);
                    }
                }
                num++;
            }
            newPacket = null;
            return true;
        }

        private bool ExecuteDelayedCommands(long now, long timeToStop)
        {
            if (this.delayedCommands != null)
            {
                if (now <= this.delayUntilTime)
                {
                    if (this.infoEnabled)
                    {
                        this.log.Info("Processing delayed commands");
                    }
                }
                else
                {
                    if (this.infoEnabled)
                    {
                        this.log.Info("Processing ALL delayed commands");
                    }
                    timeToStop = 0x7fffffffffffffffL;
                }
                try
                {
                    CommandPacket packet;
                    if (!this.ExecuteCommands(this.delayedCommands, timeToStop, out packet))
                    {
                        this.protocolAdapter.FinalizeDecodedCommandPacket(this.delayedCommands);
                        this.delayedCommands = packet;
                        return false;
                    }
                    else
                    {
                        this.protocolAdapter.FinalizeDecodedCommandPacket(this.delayedCommands);
                        this.delayedCommands = null;
                    }
                }
                catch (Exception exception)
                {
                    this.OnSocketProblem(ProblemStatus.DecodePacketError, exception);
                }
            }
            return true;
        }

        private static string GetMessage(Exception e) => 
            (!(e is TargetInvocationException) || (e.InnerException == null)) ? e.Message : (e.InnerException.GetType().Name + ": " + e.InnerException.Message);

        private void OnSocketProblem(ProblemStatus status, Exception e)
        {
            this.DisconnectOnProblemIfNeeded(status);
            throw new NetworkException("OnSocketProblem " + status, e);
        }

        public void ReadAndExecuteCommands(long maxTimeMillis, long networkMaxDelayTime)
        {
            this.infoEnabled = this.log.IsInfoEnabled;
            long now = CurrentTimeMillis();
            long timeToStop = (maxTimeMillis <= 0L) ? 0x7fffffffffffffffL : (now + maxTimeMillis);
            if (this.ExecuteDelayedCommands(now, timeToStop) && (this.IsConnected && this.socket.CanRead))
            {
                if (this.socket.AvailableLength == 0)
                {
                    this.Disconnect(ProblemStatus.ClosedByServer);
                }
                else
                {
                    int num3;
                    try
                    {
                        int availableLength = this.socket.AvailableLength;
                        this.readBuffer = BufferUtils.GetBufferWithValidSize(this.readBuffer, availableLength);
                        num3 = this.socket.Read(this.readBuffer, 0, availableLength);
                        if (this.infoEnabled)
                        {
                            this.log.InfoFormat("Received {0} byte(s).", num3);
                        }
                    }
                    catch (IOException exception)
                    {
                        this.OnSocketProblem(ProblemStatus.ReceiveError, exception);
                        return;
                    }
                    if (this.infoEnabled)
                    {
                        this.log.Info("Processing new commands");
                    }
                    try
                    {
                        CommandPacket packet;
                        this.protocolAdapter.AddChunk(this.readBuffer, num3);
                        while (this.TryDecode(out packet))
                        {
                            CommandPacket packet2;
                            bool flag = this.ExecuteCommands(packet, timeToStop, out packet2);
                            this.protocolAdapter.FinalizeDecodedCommandPacket(packet);
                            if (!flag)
                            {
                                if (this.delayedCommands == null)
                                {
                                    this.delayedCommands = packet2;
                                    this.delayUntilTime = now + networkMaxDelayTime;
                                    continue;
                                }
                                this.delayedCommands.Append(packet2);
                                this.protocolAdapter.FinalizeDecodedCommandPacket(packet2);
                            }
                        }
                    }
                    catch (Exception exception2)
                    {
                        this.OnSocketProblem(ProblemStatus.DecodePacketError, exception2);
                    }
                }
            }
        }

        public void SendCommandPacket(CommandPacket packet)
        {
            this.packetQueue.Enqueue(packet);
        }

        private bool TryDecode(out CommandPacket commandPacket)
        {
            bool flag;
            try
            {
                this.IsDecodeState = true;
                flag = this.protocolAdapter.TryDecode(out commandPacket);
            }
            finally
            {
                this.IsDecodeState = false;
            }
            return flag;
        }

        public void WriteCommands()
        {
            if (this.IsConnected && this.socket.CanWrite)
            {
                while (true)
                {
                    MemoryStreamData data;
                    while (true)
                    {
                        if (this.packetQueue.Count != 0)
                        {
                            CommandPacket packet = this.packetQueue.Dequeue();
                            try
                            {
                                data = this.protocolAdapter.Encode(packet);
                                break;
                            }
                            catch (Exception exception)
                            {
                                this.log.DebugFormat("OnSocketProblem {0}", exception.Message);
                                this.OnSocketProblem(ProblemStatus.EncodeError, exception);
                            }
                            finally
                            {
                                ClientNetworkInstancesCache.ReleaseCommandPacketWithCommandsCollection(packet);
                            }
                            return;
                        }
                        else
                        {
                            return;
                        }
                        break;
                    }
                    try
                    {
                        if (this.infoEnabled)
                        {
                            this.log.DebugFormat("WriteCommands {0}", data.Length);
                        }
                        this.socket.Write(data.GetBuffer(), 0, (int) data.Length);
                    }
                    catch (IOException exception2)
                    {
                        this.OnSocketProblem(ProblemStatus.SendError, exception2);
                    }
                    finally
                    {
                        ClientProtocolInstancesCache.ReleaseMemoryStreamData(data);
                    }
                }
            }
        }

        [Inject]
        public static Platform.System.Data.Exchange.ClientNetwork.API.ClientNetworkInstancesCache ClientNetworkInstancesCache { get; set; }

        [Inject]
        public static Platform.Library.ClientProtocol.API.ClientProtocolInstancesCache ClientProtocolInstancesCache { get; set; }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        public bool IsConnected =>
            this.socket.IsConnected;

        public bool IsDecodeState { get; private set; }

        public bool SkipThrowOnCommandExecuteException { get; set; }

        public bool SplitShareCommand
        {
            set => 
                this.protocolAdapter.SplitShareCommand = value;
        }
    }
}

