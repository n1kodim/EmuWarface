using Autofac;
using EmuWarface.Server.CryOnline.Attributes.Query;
using EmuWarface.Server.CryOnline.Xmpp;
using NLog;
using System.Reflection;

namespace EmuWarface.Server.Query
{
    public class QueryFactory
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<string, IQueryHandler> _handlers;

        public QueryFactory(IEnumerable<IQueryHandler> handlers)
        {
            _handlers = new Dictionary<string, IQueryHandler>();

            foreach (var handler in handlers)
            {
                foreach (var queryName in handler.GetType().GetCustomAttribute<QueryAttribute>().Names)
                {
                    _handlers.Add(queryName, handler);
                }
            }

            _logger.Info("Initialised {0} query handler(s)", _handlers.Count);

            // patch XmppDotNet for parse
            XmppDotNet.Xml.Factory.RegisterElement<CryOnlineQuery>();
        }

        public void RegisterHandlers(IContainer container)
        {
            var handlers = container.Resolve<IEnumerable<IQueryHandler>>();

            foreach (var handler in handlers)
            {
                foreach (var queryName in handler.GetType().GetCustomAttribute<QueryAttribute>().Names)
                {
                    _handlers.Add(queryName, handler);
                }
            }

            _logger.Info("Initialised {0} query handler(s)", _handlers.Count);
        }

        public void RegisterHandler(string queryName, IQueryHandler handler)
        {
            _handlers.Add(queryName, handler);
        }

        public IQueryHandler? GetHandler(CryOnlineQuery query)
        {
            _handlers.TryGetValue(query.Element.Name.LocalName, out IQueryHandler? res);
            return res;
            //return type != null ? _container.Resolve(type) as IQueryHandler : null;
        }
    }
}
