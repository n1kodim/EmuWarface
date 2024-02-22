using XmppDotNet.Xml;

namespace EmuWarface.Server.CryOnline.Xmpp
{
    public interface IXmppSerializable
    {
        XmppXElement Serialize();
    }
}
