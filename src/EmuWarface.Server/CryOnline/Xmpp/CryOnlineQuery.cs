using XmppDotNet.Attributes;
using XmppDotNet.Xml;
using XmppDotNet.Xmpp;

namespace EmuWarface.Server.CryOnline.Xmpp
{
    [XmppTag(Name = Tag.Query, Namespace = Namespaces.CryOnline)]
    public class CryOnlineQuery : XmppXElement
    {
        public CryOnlineQuery(XmppXElement? queryElement)
            : this()
        {
            Element = queryElement;
        }

        public CryOnlineQuery()
            : base(Namespaces.CryOnline, Tag.Query) { }

        public new XmppXElement? Element
        {
            get
            {
                return Elements().FirstOrDefault() as XmppXElement;
            }
            set
            {
                RemoveNodes();
                Add(value);
            }
        }
    }
}