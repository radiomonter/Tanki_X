using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using WebSocketSharp;

public class WebSocket
{
    private bool isActive;
    private const int WEBSOCKET_CONNECTING = 0;
    private const int WEBSOCKET_OPEN = 1;
    private WebSocket socket;
    private Queue<byte[]> messages = new Queue<byte[]>();
    private string error;

    public void Close()
    {
        this.socket.Close();
    }

    public void ConnectAsync(string url, Action completeCallback)
    {
        <ConnectAsync>c__AnonStorey0 storey = new <ConnectAsync>c__AnonStorey0 {
            completeCallback = completeCallback,
            $this = this
        };
        if (this.isActive)
        {
            throw new Exception("Connection in progress");
        }
        this.isActive = true;
        this.socket = new WebSocket(url, new string[0]);
        this.socket.OnOpen += new EventHandler(storey.<>m__0);
        this.socket.OnMessage += new EventHandler<MessageEventArgs>(storey.<>m__1);
        this.socket.OnError += new EventHandler<ErrorEventArgs>(storey.<>m__2);
        this.socket.OnClose += new EventHandler<CloseEventArgs>(storey.<>m__3);
        this.socket.ConnectAsync();
    }

    public string GetError() => 
        this.error;

    public int Receive(byte[] buffer)
    {
        if (this.messages.Count == 0)
        {
            return 0;
        }
        byte[] src = this.messages.Dequeue();
        Buffer.BlockCopy(src, 0, buffer, 0, src.Length);
        return src.Length;
    }

    public void Send(byte[] buffer)
    {
        this.socket.Send(buffer);
    }

    public bool IsConnected { get; private set; }

    public int AvailableLength =>
        (this.messages.Count != 0) ? this.messages.Peek().Length : 0;

    [CompilerGenerated]
    private sealed class <ConnectAsync>c__AnonStorey0
    {
        internal Action completeCallback;
        internal WebSocket $this;

        internal void <>m__0(object sender, EventArgs e)
        {
            this.$this.IsConnected = true;
            this.completeCallback();
        }

        internal void <>m__1(object sender, MessageEventArgs e)
        {
            this.$this.messages.Enqueue(e.RawData);
        }

        internal void <>m__2(object sender, ErrorEventArgs e)
        {
            this.$this.error = e.Message;
        }

        internal void <>m__3(object sender, CloseEventArgs e)
        {
            if (!this.$this.IsConnected)
            {
                this.completeCallback();
            }
            else
            {
                this.$this.IsConnected = false;
            }
        }
    }
}

