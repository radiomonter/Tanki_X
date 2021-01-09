namespace log4net.Appender
{
    using log4net.Core;
    using log4net.Util;
    using System;
    using System.Globalization;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public class UdpAppender : AppenderSkeleton
    {
        private IPAddress m_remoteAddress;
        private int m_remotePort;
        private IPEndPoint m_remoteEndPoint;
        private int m_localPort;
        private UdpClient m_client;
        private System.Text.Encoding m_encoding = System.Text.Encoding.Default;

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            if (this.RemoteAddress == null)
            {
                throw new ArgumentNullException("The required property 'Address' was not specified.");
            }
            if ((this.RemotePort < 0) || (this.RemotePort > 0xffff))
            {
                string[] textArray1 = new string[] { "The RemotePort is less than ", 0.ToString(NumberFormatInfo.InvariantInfo), " or greater than ", 0xffff.ToString(NumberFormatInfo.InvariantInfo), "." };
                throw SystemInfo.CreateArgumentOutOfRangeException("this.RemotePort", this.RemotePort, string.Concat(textArray1));
            }
            if ((this.LocalPort == 0) || ((this.LocalPort >= 0) && (this.LocalPort <= 0xffff)))
            {
                this.RemoteEndPoint = new IPEndPoint(this.RemoteAddress, this.RemotePort);
                this.InitializeClientConnection();
            }
            else
            {
                string[] textArray2 = new string[] { "The LocalPort is less than ", 0.ToString(NumberFormatInfo.InvariantInfo), " or greater than ", 0xffff.ToString(NumberFormatInfo.InvariantInfo), "." };
                throw SystemInfo.CreateArgumentOutOfRangeException("this.LocalPort", this.LocalPort, string.Concat(textArray2));
            }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                byte[] bytes = this.m_encoding.GetBytes(base.RenderLoggingEvent(loggingEvent).ToCharArray());
                this.Client.Send(bytes, bytes.Length, this.RemoteEndPoint);
            }
            catch (Exception exception)
            {
                object[] objArray1 = new object[] { "Unable to send logging event to remote host ", this.RemoteAddress.ToString(), " on port ", this.RemotePort, "." };
                this.ErrorHandler.Error(string.Concat(objArray1), exception, ErrorCode.WriteFailure);
            }
        }

        protected virtual void InitializeClientConnection()
        {
            try
            {
                this.Client = (this.LocalPort != 0) ? new UdpClient(this.LocalPort, this.RemoteAddress.AddressFamily) : new UdpClient(this.RemoteAddress.AddressFamily);
            }
            catch (Exception exception)
            {
                this.ErrorHandler.Error("Could not initialize the UdpClient connection on port " + this.LocalPort.ToString(NumberFormatInfo.InvariantInfo) + ".", exception, ErrorCode.GenericFailure);
                this.Client = null;
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
            if (this.Client != null)
            {
                this.Client.Close();
                this.Client = null;
            }
        }

        public IPAddress RemoteAddress
        {
            get => 
                this.m_remoteAddress;
            set => 
                this.m_remoteAddress = value;
        }

        public int RemotePort
        {
            get => 
                this.m_remotePort;
            set
            {
                if ((value >= 0) && (value <= 0xffff))
                {
                    this.m_remotePort = value;
                }
                else
                {
                    string[] textArray1 = new string[] { "The value specified is less than ", 0.ToString(NumberFormatInfo.InvariantInfo), " or greater than ", 0xffff.ToString(NumberFormatInfo.InvariantInfo), "." };
                    throw SystemInfo.CreateArgumentOutOfRangeException("value", value, string.Concat(textArray1));
                }
            }
        }

        public int LocalPort
        {
            get => 
                this.m_localPort;
            set
            {
                if ((value == 0) || ((value >= 0) && (value <= 0xffff)))
                {
                    this.m_localPort = value;
                }
                else
                {
                    string[] textArray1 = new string[] { "The value specified is less than ", 0.ToString(NumberFormatInfo.InvariantInfo), " or greater than ", 0xffff.ToString(NumberFormatInfo.InvariantInfo), "." };
                    throw SystemInfo.CreateArgumentOutOfRangeException("value", value, string.Concat(textArray1));
                }
            }
        }

        public System.Text.Encoding Encoding
        {
            get => 
                this.m_encoding;
            set => 
                this.m_encoding = value;
        }

        protected UdpClient Client
        {
            get => 
                this.m_client;
            set => 
                this.m_client = value;
        }

        protected IPEndPoint RemoteEndPoint
        {
            get => 
                this.m_remoteEndPoint;
            set => 
                this.m_remoteEndPoint = value;
        }

        protected override bool RequiresLayout =>
            true;
    }
}

