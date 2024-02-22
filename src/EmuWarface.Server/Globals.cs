using System.Security.Cryptography.X509Certificates;
using XmppDotNet;

namespace EmuWarface
{
	public class Globals
	{
		// Warface JID's 
		public const string XmppDomain = "warface";
		public const string MasterServerNode = "masterserver";
        public const string CryOnlineNS = "urn:cryonline:k01";

        public static readonly Jid K01Jid = new Jid("k01." + XmppDomain);
		public static readonly Jid MSJid = new Jid("ms." + XmppDomain);

        public static readonly X509Certificate2 Certificate = new X509Certificate2("emu.pfx");
    }
}