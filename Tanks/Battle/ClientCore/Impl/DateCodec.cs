namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class DateCodec : NotOptionalCodec
    {
        public override object Decode(ProtocolBuffer protocolBuffer)
        {
            long serverTime = protocolBuffer.Reader.ReadInt64();
            long diffToServer = TimeService.DiffToServer;
            float num3 = Date.FromServerTime(diffToServer, serverTime);
            LoggerProvider.GetLogger(this).InfoFormat("Decode: serverTime={0} diffToServer={1} unityTime={2}", serverTime, diffToServer, num3);
            return new Date(num3);
        }

        public override void Encode(ProtocolBuffer protocolBuffer, object data)
        {
            base.Encode(protocolBuffer, data);
            long num = ((Date) data).ToServerTime(TimeService.DiffToServer);
            protocolBuffer.Writer.Write(num);
        }

        public override void Init(Protocol protocol)
        {
        }

        [Inject]
        public static Tanks.Battle.ClientCore.API.TimeService TimeService { get; set; }
    }
}

