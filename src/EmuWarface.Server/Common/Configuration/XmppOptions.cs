namespace EmuWarface.Server.Common.Configuration
{
    public class XmppOptions
    {
        public const string Xmpp = "Xmpp";

        public string Host { get; set; }
        public int Port { get; set; }
        public bool AllowAnyone { get; set; }
    }
}
