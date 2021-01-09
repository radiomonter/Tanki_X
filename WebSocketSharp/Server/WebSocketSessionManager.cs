namespace WebSocketSharp.Server
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Timers;
    using WebSocketSharp;

    public class WebSocketSessionManager
    {
        private volatile bool _clean;
        private object _forSweep;
        private Logger _logger;
        private Dictionary<string, IWebSocketSession> _sessions;
        private volatile ServerState _state;
        private volatile bool _sweeping;
        private Timer _sweepTimer;
        private object _sync;
        private TimeSpan _waitTime;

        internal WebSocketSessionManager() : this(new Logger())
        {
        }

        internal WebSocketSessionManager(Logger logger)
        {
            this._logger = logger;
            this._clean = true;
            this._forSweep = new object();
            this._sessions = new Dictionary<string, IWebSocketSession>();
            this._state = 0;
            this._sync = ((ICollection) this._sessions).SyncRoot;
            this._waitTime = TimeSpan.FromSeconds(1.0);
            this.setSweepTimer(60000.0);
        }

        internal string Add(IWebSocketSession session)
        {
            string str;
            lock (this._sync)
            {
                if (this._state != 1)
                {
                    str = null;
                }
                else
                {
                    string key = createID();
                    this._sessions.Add(key, session);
                    str = key;
                }
            }
            return str;
        }

        private void broadcast(Opcode opcode, byte[] data, Action completed)
        {
            Dictionary<CompressionMethod, byte[]> cache = new Dictionary<CompressionMethod, byte[]>();
            try
            {
                this.Broadcast(opcode, data, cache);
                if (completed != null)
                {
                    completed();
                }
            }
            catch (Exception exception)
            {
                this._logger.Fatal(exception.ToString());
            }
            finally
            {
                cache.Clear();
            }
        }

        private void broadcast(Opcode opcode, Stream stream, Action completed)
        {
            Dictionary<CompressionMethod, Stream> cache = new Dictionary<CompressionMethod, Stream>();
            try
            {
                this.Broadcast(opcode, stream, cache);
                if (completed != null)
                {
                    completed();
                }
            }
            catch (Exception exception)
            {
                this._logger.Fatal(exception.ToString());
            }
            finally
            {
                foreach (Stream stream2 in cache.Values)
                {
                    stream2.Dispose();
                }
                cache.Clear();
            }
        }

        public void Broadcast(byte[] data)
        {
            string message = this._state.CheckIfAvailable(false, true, false) ?? WebSocket.CheckSendParameter(data);
            if (message != null)
            {
                this._logger.Error(message);
            }
            else if (data.LongLength <= WebSocket.FragmentLength)
            {
                this.broadcast(Opcode.Binary, data, null);
            }
            else
            {
                this.broadcast(Opcode.Binary, new MemoryStream(data), null);
            }
        }

        public void Broadcast(string data)
        {
            string message = this._state.CheckIfAvailable(false, true, false) ?? WebSocket.CheckSendParameter(data);
            if (message != null)
            {
                this._logger.Error(message);
            }
            else
            {
                byte[] buffer = data.UTF8Encode();
                if (buffer.LongLength <= WebSocket.FragmentLength)
                {
                    this.broadcast(Opcode.Text, buffer, null);
                }
                else
                {
                    this.broadcast(Opcode.Text, new MemoryStream(buffer), null);
                }
            }
        }

        internal void Broadcast(Opcode opcode, byte[] data, Dictionary<CompressionMethod, byte[]> cache)
        {
            foreach (IWebSocketSession session in this.Sessions)
            {
                if (this._state != 1)
                {
                    break;
                }
                session.Context.WebSocket.Send(opcode, data, cache);
            }
        }

        internal void Broadcast(Opcode opcode, Stream stream, Dictionary<CompressionMethod, Stream> cache)
        {
            foreach (IWebSocketSession session in this.Sessions)
            {
                if (this._state != 1)
                {
                    break;
                }
                session.Context.WebSocket.Send(opcode, stream, cache);
            }
        }

        private void broadcastAsync(Opcode opcode, byte[] data, Action completed)
        {
            <broadcastAsync>c__AnonStorey2 storey = new <broadcastAsync>c__AnonStorey2 {
                opcode = opcode,
                data = data,
                completed = completed,
                $this = this
            };
            ThreadPool.QueueUserWorkItem(new WaitCallback(storey.<>m__0));
        }

        private void broadcastAsync(Opcode opcode, Stream stream, Action completed)
        {
            <broadcastAsync>c__AnonStorey3 storey = new <broadcastAsync>c__AnonStorey3 {
                opcode = opcode,
                stream = stream,
                completed = completed,
                $this = this
            };
            ThreadPool.QueueUserWorkItem(new WaitCallback(storey.<>m__0));
        }

        public void BroadcastAsync(byte[] data, Action completed)
        {
            string message = this._state.CheckIfAvailable(false, true, false) ?? WebSocket.CheckSendParameter(data);
            if (message != null)
            {
                this._logger.Error(message);
            }
            else if (data.LongLength <= WebSocket.FragmentLength)
            {
                this.broadcastAsync(Opcode.Binary, data, completed);
            }
            else
            {
                this.broadcastAsync(Opcode.Binary, new MemoryStream(data), completed);
            }
        }

        public void BroadcastAsync(string data, Action completed)
        {
            string message = this._state.CheckIfAvailable(false, true, false) ?? WebSocket.CheckSendParameter(data);
            if (message != null)
            {
                this._logger.Error(message);
            }
            else
            {
                byte[] buffer = data.UTF8Encode();
                if (buffer.LongLength <= WebSocket.FragmentLength)
                {
                    this.broadcastAsync(Opcode.Text, buffer, completed);
                }
                else
                {
                    this.broadcastAsync(Opcode.Text, new MemoryStream(buffer), completed);
                }
            }
        }

        public void BroadcastAsync(Stream stream, int length, Action completed)
        {
            <BroadcastAsync>c__AnonStorey4 storey = new <BroadcastAsync>c__AnonStorey4 {
                length = length,
                completed = completed,
                $this = this
            };
            string message = this._state.CheckIfAvailable(false, true, false) ?? WebSocket.CheckSendParameters(stream, storey.length);
            if (message != null)
            {
                this._logger.Error(message);
            }
            else
            {
                stream.ReadBytesAsync(storey.length, new Action<byte[]>(storey.<>m__0), new Action<Exception>(storey.<>m__1));
            }
        }

        public Dictionary<string, bool> Broadping()
        {
            string message = this._state.CheckIfAvailable(false, true, false);
            if (message == null)
            {
                return this.Broadping(WebSocketFrame.EmptyPingBytes, this._waitTime);
            }
            this._logger.Error(message);
            return null;
        }

        public Dictionary<string, bool> Broadping(string message)
        {
            if ((message == null) || (message.Length == 0))
            {
                return this.Broadping();
            }
            byte[] bytes = null;
            string str = this._state.CheckIfAvailable(false, true, false) ?? WebSocket.CheckPingParameter(message, out bytes);
            if (str == null)
            {
                return this.Broadping(WebSocketFrame.CreatePingFrame(bytes, false).ToArray(), this._waitTime);
            }
            this._logger.Error(str);
            return null;
        }

        internal Dictionary<string, bool> Broadping(byte[] frameAsBytes, TimeSpan timeout)
        {
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
            foreach (IWebSocketSession session in this.Sessions)
            {
                if (this._state != 1)
                {
                    break;
                }
                dictionary.Add(session.ID, session.Context.WebSocket.Ping(frameAsBytes, timeout));
            }
            return dictionary;
        }

        public void CloseSession(string id)
        {
            IWebSocketSession session;
            if (this.TryGetSession(id, out session))
            {
                session.Context.WebSocket.Close();
            }
        }

        public void CloseSession(string id, ushort code, string reason)
        {
            IWebSocketSession session;
            if (this.TryGetSession(id, out session))
            {
                session.Context.WebSocket.Close(code, reason);
            }
        }

        public void CloseSession(string id, CloseStatusCode code, string reason)
        {
            IWebSocketSession session;
            if (this.TryGetSession(id, out session))
            {
                session.Context.WebSocket.Close(code, reason);
            }
        }

        private static string createID() => 
            Guid.NewGuid().ToString("N");

        public bool PingTo(string id)
        {
            IWebSocketSession session;
            return (this.TryGetSession(id, out session) && session.Context.WebSocket.Ping());
        }

        public bool PingTo(string message, string id)
        {
            IWebSocketSession session;
            return (this.TryGetSession(id, out session) && session.Context.WebSocket.Ping(message));
        }

        internal bool Remove(string id)
        {
            lock (this._sync)
            {
                return this._sessions.Remove(id);
            }
        }

        public void SendTo(byte[] data, string id)
        {
            IWebSocketSession session;
            if (this.TryGetSession(id, out session))
            {
                session.Context.WebSocket.Send(data);
            }
        }

        public void SendTo(string data, string id)
        {
            IWebSocketSession session;
            if (this.TryGetSession(id, out session))
            {
                session.Context.WebSocket.Send(data);
            }
        }

        public void SendToAsync(byte[] data, string id, Action<bool> completed)
        {
            IWebSocketSession session;
            if (this.TryGetSession(id, out session))
            {
                session.Context.WebSocket.SendAsync(data, completed);
            }
        }

        public void SendToAsync(string data, string id, Action<bool> completed)
        {
            IWebSocketSession session;
            if (this.TryGetSession(id, out session))
            {
                session.Context.WebSocket.SendAsync(data, completed);
            }
        }

        public void SendToAsync(Stream stream, int length, string id, Action<bool> completed)
        {
            IWebSocketSession session;
            if (this.TryGetSession(id, out session))
            {
                session.Context.WebSocket.SendAsync(stream, length, completed);
            }
        }

        private void setSweepTimer(double interval)
        {
            this._sweepTimer = new Timer(interval);
            this._sweepTimer.Elapsed += (sender, e) => this.Sweep();
        }

        internal void Start()
        {
            lock (this._sync)
            {
                this._sweepTimer.Enabled = this._clean;
                this._state = 1;
            }
        }

        internal void Stop(CloseEventArgs e, byte[] frameAsBytes, bool receive)
        {
            lock (this._sync)
            {
                this._state = 2;
                this._sweepTimer.Enabled = false;
                foreach (IWebSocketSession session in this._sessions.Values.ToList<IWebSocketSession>())
                {
                    session.Context.WebSocket.Close(e, frameAsBytes, receive);
                }
                this._state = 3;
            }
        }

        public void Sweep()
        {
            if ((this._state == 1) && (!this._sweeping && (this.Count != 0)))
            {
                lock (this._forSweep)
                {
                    this._sweeping = true;
                    foreach (string str in this.InactiveIDs)
                    {
                        if (this._state != 1)
                        {
                            break;
                        }
                        lock (this._sync)
                        {
                            IWebSocketSession session;
                            if (this._sessions.TryGetValue(str, out session))
                            {
                                WebSocketState state = session.State;
                                if (state == WebSocketState.Open)
                                {
                                    session.Context.WebSocket.Close(CloseStatusCode.ProtocolError);
                                }
                                else if (state != WebSocketState.Closing)
                                {
                                    this._sessions.Remove(str);
                                }
                            }
                        }
                    }
                    this._sweeping = false;
                }
            }
        }

        private bool tryGetSession(string id, out IWebSocketSession session)
        {
            bool flag;
            lock (this._sync)
            {
                flag = this._sessions.TryGetValue(id, out session);
            }
            if (!flag)
            {
                this._logger.Error("A session with the specified ID isn't found:\n  ID: " + id);
            }
            return flag;
        }

        public bool TryGetSession(string id, out IWebSocketSession session)
        {
            string message = this._state.CheckIfAvailable(false, true, false) ?? id.CheckIfValidSessionID();
            if (message == null)
            {
                return this.tryGetSession(id, out session);
            }
            this._logger.Error(message);
            session = null;
            return false;
        }

        internal ServerState State =>
            this._state;

        public IEnumerable<string> ActiveIDs =>
            new <>c__Iterator0 { 
                $this=this,
                $PC=-2
            };

        public int Count
        {
            get
            {
                lock (this._sync)
                {
                    return this._sessions.Count;
                }
            }
        }

        public IEnumerable<string> IDs
        {
            get
            {
                if (this._state == 2)
                {
                    return new string[0];
                }
                lock (this._sync)
                {
                    return this._sessions.Keys.ToList<string>();
                }
            }
        }

        public IEnumerable<string> InactiveIDs =>
            new <>c__Iterator1 { 
                $this=this,
                $PC=-2
            };

        public IWebSocketSession this[string id]
        {
            get
            {
                IWebSocketSession session;
                this.TryGetSession(id, out session);
                return session;
            }
        }

        public bool KeepClean
        {
            get => 
                this._clean;
            internal set
            {
                if (value ^ this._clean)
                {
                    this._clean = value;
                    if (this._state == 1)
                    {
                        this._sweepTimer.Enabled = value;
                    }
                }
            }
        }

        public IEnumerable<IWebSocketSession> Sessions
        {
            get
            {
                if (this._state == 2)
                {
                    return new IWebSocketSession[0];
                }
                lock (this._sync)
                {
                    return this._sessions.Values.ToList<IWebSocketSession>();
                }
            }
        }

        public TimeSpan WaitTime
        {
            get => 
                this._waitTime;
            internal set
            {
                if (value != this._waitTime)
                {
                    this._waitTime = value;
                    foreach (IWebSocketSession session in this.Sessions)
                    {
                        session.Context.WebSocket.WaitTime = value;
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
        {
            internal Dictionary<string, bool>.Enumerator $locvar0;
            internal KeyValuePair<string, bool> <res>__1;
            internal WebSocketSessionManager $this;
            internal string $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$disposing = true;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            this.$locvar0.Dispose();
                        }
                        break;

                    default:
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.$locvar0 = this.$this.Broadping(WebSocketFrame.EmptyPingBytes, this.$this._waitTime).GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto TR_0000;
                }
                try
                {
                    switch (num)
                    {
                        default:
                            while (true)
                            {
                                if (this.$locvar0.MoveNext())
                                {
                                    this.<res>__1 = this.$locvar0.Current;
                                    if (!this.<res>__1.Value)
                                    {
                                        continue;
                                    }
                                    this.$current = this.<res>__1.Key;
                                    if (!this.$disposing)
                                    {
                                        this.$PC = 1;
                                    }
                                    flag = true;
                                }
                                else
                                {
                                    this.$PC = -1;
                                    goto TR_0000;
                                }
                                break;
                            }
                            break;
                    }
                }
                finally
                {
                    if (flag)
                    {
                    }
                    this.$locvar0.Dispose();
                }
                return true;
            TR_0000:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new WebSocketSessionManager.<>c__Iterator0 { $this = this.$this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();

            string IEnumerator<string>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        [CompilerGenerated]
        private sealed class <>c__Iterator1 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
        {
            internal Dictionary<string, bool>.Enumerator $locvar0;
            internal KeyValuePair<string, bool> <res>__1;
            internal WebSocketSessionManager $this;
            internal string $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$disposing = true;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            this.$locvar0.Dispose();
                        }
                        break;

                    default:
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.$locvar0 = this.$this.Broadping(WebSocketFrame.EmptyPingBytes, this.$this._waitTime).GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto TR_0000;
                }
                try
                {
                    switch (num)
                    {
                        default:
                            while (true)
                            {
                                if (this.$locvar0.MoveNext())
                                {
                                    this.<res>__1 = this.$locvar0.Current;
                                    if (this.<res>__1.Value)
                                    {
                                        continue;
                                    }
                                    this.$current = this.<res>__1.Key;
                                    if (!this.$disposing)
                                    {
                                        this.$PC = 1;
                                    }
                                    flag = true;
                                }
                                else
                                {
                                    this.$PC = -1;
                                    goto TR_0000;
                                }
                                break;
                            }
                            break;
                    }
                }
                finally
                {
                    if (flag)
                    {
                    }
                    this.$locvar0.Dispose();
                }
                return true;
            TR_0000:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new WebSocketSessionManager.<>c__Iterator1 { $this = this.$this };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();

            string IEnumerator<string>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        [CompilerGenerated]
        private sealed class <broadcastAsync>c__AnonStorey2
        {
            internal Opcode opcode;
            internal byte[] data;
            internal Action completed;
            internal WebSocketSessionManager $this;

            internal void <>m__0(object state)
            {
                this.$this.broadcast(this.opcode, this.data, this.completed);
            }
        }

        [CompilerGenerated]
        private sealed class <broadcastAsync>c__AnonStorey3
        {
            internal Opcode opcode;
            internal Stream stream;
            internal Action completed;
            internal WebSocketSessionManager $this;

            internal void <>m__0(object state)
            {
                this.$this.broadcast(this.opcode, this.stream, this.completed);
            }
        }

        [CompilerGenerated]
        private sealed class <BroadcastAsync>c__AnonStorey4
        {
            internal int length;
            internal Action completed;
            internal WebSocketSessionManager $this;

            internal void <>m__0(byte[] data)
            {
                int length = data.Length;
                if (length == 0)
                {
                    this.$this._logger.Error("The data cannot be read from 'stream'.");
                }
                else
                {
                    if (length < this.length)
                    {
                        this.$this._logger.Warn($"The data with 'length' cannot be read from 'stream':
  expected: {this.length}
  actual: {length}");
                    }
                    if (length <= WebSocket.FragmentLength)
                    {
                        this.$this.broadcast(Opcode.Binary, data, this.completed);
                    }
                    else
                    {
                        this.$this.broadcast(Opcode.Binary, new MemoryStream(data), this.completed);
                    }
                }
            }

            internal void <>m__1(Exception ex)
            {
                this.$this._logger.Fatal(ex.ToString());
            }
        }
    }
}

