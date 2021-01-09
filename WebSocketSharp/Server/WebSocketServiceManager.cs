namespace WebSocketSharp.Server
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using WebSocketSharp;
    using WebSocketSharp.Net;

    public class WebSocketServiceManager
    {
        private volatile bool _clean;
        private Dictionary<string, WebSocketServiceHost> _hosts;
        private Logger _logger;
        private volatile ServerState _state;
        private object _sync;
        private TimeSpan _waitTime;

        internal WebSocketServiceManager() : this(new Logger())
        {
        }

        internal WebSocketServiceManager(Logger logger)
        {
            this._logger = logger;
            this._clean = true;
            this._hosts = new Dictionary<string, WebSocketServiceHost>();
            this._state = 0;
            this._sync = ((ICollection) this._hosts).SyncRoot;
            this._waitTime = TimeSpan.FromSeconds(1.0);
        }

        internal void Add<TBehavior>(string path, Func<TBehavior> initializer) where TBehavior: WebSocketBehavior
        {
            lock (this._sync)
            {
                WebSocketServiceHost host;
                path = HttpUtility.UrlDecode(path).TrimEndSlash();
                if (this._hosts.TryGetValue(path, out host))
                {
                    this._logger.Error("A WebSocket service with the specified path already exists:\n  path: " + path);
                }
                else
                {
                    host = new WebSocketServiceHost<TBehavior>(path, initializer, this._logger);
                    if (!this._clean)
                    {
                        host.KeepClean = false;
                    }
                    if (this._waitTime != host.WaitTime)
                    {
                        host.WaitTime = this._waitTime;
                    }
                    if (this._state == 1)
                    {
                        host.Start();
                    }
                    this._hosts.Add(path, host);
                }
            }
        }

        private void broadcast(Opcode opcode, byte[] data, Action completed)
        {
            Dictionary<CompressionMethod, byte[]> cache = new Dictionary<CompressionMethod, byte[]>();
            try
            {
                foreach (WebSocketServiceHost host in this.Hosts)
                {
                    if (this._state != 1)
                    {
                        break;
                    }
                    host.Sessions.Broadcast(opcode, data, cache);
                }
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
                foreach (WebSocketServiceHost host in this.Hosts)
                {
                    if (this._state != 1)
                    {
                        break;
                    }
                    host.Sessions.Broadcast(opcode, stream, cache);
                }
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

        private void broadcastAsync(Opcode opcode, byte[] data, Action completed)
        {
            <broadcastAsync>c__AnonStorey0 storey = new <broadcastAsync>c__AnonStorey0 {
                opcode = opcode,
                data = data,
                completed = completed,
                $this = this
            };
            ThreadPool.QueueUserWorkItem(new WaitCallback(storey.<>m__0));
        }

        private void broadcastAsync(Opcode opcode, Stream stream, Action completed)
        {
            <broadcastAsync>c__AnonStorey1 storey = new <broadcastAsync>c__AnonStorey1 {
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
            <BroadcastAsync>c__AnonStorey2 storey = new <BroadcastAsync>c__AnonStorey2 {
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

        private Dictionary<string, Dictionary<string, bool>> broadping(byte[] frameAsBytes, TimeSpan timeout)
        {
            Dictionary<string, Dictionary<string, bool>> dictionary = new Dictionary<string, Dictionary<string, bool>>();
            foreach (WebSocketServiceHost host in this.Hosts)
            {
                if (this._state != 1)
                {
                    break;
                }
                dictionary.Add(host.Path, host.Sessions.Broadping(frameAsBytes, timeout));
            }
            return dictionary;
        }

        public Dictionary<string, Dictionary<string, bool>> Broadping()
        {
            string message = this._state.CheckIfAvailable(false, true, false);
            if (message == null)
            {
                return this.broadping(WebSocketFrame.EmptyPingBytes, this._waitTime);
            }
            this._logger.Error(message);
            return null;
        }

        public Dictionary<string, Dictionary<string, bool>> Broadping(string message)
        {
            if ((message == null) || (message.Length == 0))
            {
                return this.Broadping();
            }
            byte[] bytes = null;
            string str = this._state.CheckIfAvailable(false, true, false) ?? WebSocket.CheckPingParameter(message, out bytes);
            if (str == null)
            {
                return this.broadping(WebSocketFrame.CreatePingFrame(bytes, false).ToArray(), this._waitTime);
            }
            this._logger.Error(str);
            return null;
        }

        internal bool InternalTryGetServiceHost(string path, out WebSocketServiceHost host)
        {
            bool flag;
            lock (this._sync)
            {
                path = HttpUtility.UrlDecode(path).TrimEndSlash();
                flag = this._hosts.TryGetValue(path, out host);
            }
            if (!flag)
            {
                this._logger.Error("A WebSocket service with the specified path isn't found:\n  path: " + path);
            }
            return flag;
        }

        internal bool Remove(string path)
        {
            WebSocketServiceHost host;
            lock (this._sync)
            {
                path = HttpUtility.UrlDecode(path).TrimEndSlash();
                if (this._hosts.TryGetValue(path, out host))
                {
                    this._hosts.Remove(path);
                }
                else
                {
                    this._logger.Error("A WebSocket service with the specified path isn't found:\n  path: " + path);
                    return false;
                }
            }
            if (host.State == ServerState.Start)
            {
                host.Stop(0x3e9, null);
            }
            return true;
        }

        internal void Start()
        {
            lock (this._sync)
            {
                foreach (WebSocketServiceHost host in this._hosts.Values)
                {
                    host.Start();
                }
                this._state = 1;
            }
        }

        internal void Stop(CloseEventArgs e, bool send, bool receive)
        {
            lock (this._sync)
            {
                this._state = 2;
                byte[] frameAsBytes = !send ? null : WebSocketFrame.CreateCloseFrame(e.PayloadData, false).ToArray();
                foreach (WebSocketServiceHost host in this._hosts.Values)
                {
                    host.Sessions.Stop(e, frameAsBytes, receive);
                }
                this._hosts.Clear();
                this._state = 3;
            }
        }

        public bool TryGetServiceHost(string path, out WebSocketServiceHost host)
        {
            string message = this._state.CheckIfAvailable(false, true, false) ?? path.CheckIfValidServicePath();
            if (message == null)
            {
                return this.InternalTryGetServiceHost(path, out host);
            }
            this._logger.Error(message);
            host = null;
            return false;
        }

        public int Count
        {
            get
            {
                lock (this._sync)
                {
                    return this._hosts.Count;
                }
            }
        }

        public IEnumerable<WebSocketServiceHost> Hosts
        {
            get
            {
                lock (this._sync)
                {
                    return this._hosts.Values.ToList<WebSocketServiceHost>();
                }
            }
        }

        public WebSocketServiceHost this[string path]
        {
            get
            {
                WebSocketServiceHost host;
                this.TryGetServiceHost(path, out host);
                return host;
            }
        }

        public bool KeepClean
        {
            get => 
                this._clean;
            internal set
            {
                lock (this._sync)
                {
                    if (value ^ this._clean)
                    {
                        this._clean = value;
                        foreach (WebSocketServiceHost host in this._hosts.Values)
                        {
                            host.KeepClean = value;
                        }
                    }
                }
            }
        }

        public IEnumerable<string> Paths
        {
            get
            {
                lock (this._sync)
                {
                    return this._hosts.Keys.ToList<string>();
                }
            }
        }

        public int SessionCount
        {
            get
            {
                int num = 0;
                foreach (WebSocketServiceHost host in this.Hosts)
                {
                    if (this._state != 1)
                    {
                        break;
                    }
                    num += host.Sessions.Count;
                }
                return num;
            }
        }

        public TimeSpan WaitTime
        {
            get => 
                this._waitTime;
            internal set
            {
                lock (this._sync)
                {
                    if (!(value == this._waitTime))
                    {
                        this._waitTime = value;
                        foreach (WebSocketServiceHost host in this._hosts.Values)
                        {
                            host.WaitTime = value;
                        }
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <broadcastAsync>c__AnonStorey0
        {
            internal Opcode opcode;
            internal byte[] data;
            internal Action completed;
            internal WebSocketServiceManager $this;

            internal void <>m__0(object state)
            {
                this.$this.broadcast(this.opcode, this.data, this.completed);
            }
        }

        [CompilerGenerated]
        private sealed class <broadcastAsync>c__AnonStorey1
        {
            internal Opcode opcode;
            internal Stream stream;
            internal Action completed;
            internal WebSocketServiceManager $this;

            internal void <>m__0(object state)
            {
                this.$this.broadcast(this.opcode, this.stream, this.completed);
            }
        }

        [CompilerGenerated]
        private sealed class <BroadcastAsync>c__AnonStorey2
        {
            internal int length;
            internal Action completed;
            internal WebSocketServiceManager $this;

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

