namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    public class NewsItem
    {
        public override string ToString()
        {
            object[] args = new object[11];
            args[0] = this.Date;
            args[1] = this.HeaderText;
            args[2] = this.ShortText;
            args[3] = this.LongText;
            args[4] = this.PreviewImageUrl;
            args[5] = this.PreviewImageGuid;
            args[6] = this.CentralIconGuid;
            args[7] = this.PreviewImageFitInParent;
            args[8] = this.ExternalUrl;
            args[9] = this.InternalUrl;
            args[10] = this.Layout;
            return string.Format("Date: {0}, HeaderText: {1}, ShortText: {2}, LongText: {3}, PreviewImageUrl: {4}, PreviewImageGuid: {5}, CentralIconGuid: {6}, PreviewImageFitInParent: {7}, ExternalUrl: {8}, InternalUrl: {9}, Layout: {10}", args);
        }

        [ProtocolOptional]
        public DateTime Date { get; set; }

        [ProtocolOptional]
        public string HeaderText { get; set; }

        [ProtocolOptional]
        public string ShortText { get; set; }

        [ProtocolOptional]
        public string LongText { get; set; }

        [ProtocolOptional]
        public string PreviewImageUrl { get; set; }

        [ProtocolOptional]
        public string PreviewImageGuid { get; set; }

        [ProtocolOptional]
        public string CentralIconGuid { get; set; }

        [ProtocolOptional]
        public string Tooltip { get; set; }

        public bool PreviewImageFitInParent { get; set; }

        [ProtocolOptional]
        public string ExternalUrl { get; set; }

        [ProtocolOptional]
        public string InternalUrl { get; set; }

        public NewsItemLayout Layout { get; set; }
    }
}

