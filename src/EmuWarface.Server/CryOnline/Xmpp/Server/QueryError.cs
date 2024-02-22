using XmppDotNet.Xmpp.Server;
using XmppDotNet.Xmpp.Base;

namespace EmuWarface.Server.CryOnline.Xmpp.Server
{
    public class QueryError : XmppDotNet.Xmpp.Server.Error
    {
        public QueryError(int customeCode)
            : base(ErrorCondition.InternalServerError, ErrorType.Continue)
        {
            SetAttribute("code", customeCode);
            Text = "Query processing error";
        }
    }
}
