using Autofac;
using EmuWarface.Server.Common.Attributes;
using NLog;
using System.Reflection;

namespace EmuWarface.Server.Query
{
    [Service]
    public class QueryFactory
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<string, QueryHandler> _handlers;

        public QueryFactory(IEnumerable<QueryHandler> handlers)
        {
            _handlers = new Dictionary<string, QueryHandler>();

            foreach (var handler in handlers)
            {
                foreach (var queryName in handler.GetType().GetCustomAttribute<QueryAttribute>().Names)
                {
                    _handlers.Add(queryName, handler);
                }
            }

            _logger.Info("Initialised {0} query handler(s)", _handlers.Count);

            // patch XmppDotNet for parse
            // XmppDotNet.Xml.Factory.RegisterElement<CryOnlineQuery>();
        }

        public void RegisterHandlers(IContainer container)
        {
            var handlers = container.Resolve<IEnumerable<QueryHandler>>();

            foreach (var handler in handlers)
            {
                foreach (var queryName in handler.GetType().GetCustomAttribute<QueryAttribute>().Names)
                {
                    _handlers.Add(queryName, handler);
                }
            }

            _logger.Info("Initialised {0} query handler(s)", _handlers.Count);
        }

        public void RegisterHandler(string queryName, QueryHandler handler)
        {
            _handlers.Add(queryName, handler);
        }

        public QueryHandler? GetHandler(string queryName)
        {
            _handlers.TryGetValue(queryName, out QueryHandler? res);
            return res;
        }

        public T? GetHandler<T>() where T : QueryHandler
        {
            return (T)_handlers.FirstOrDefault(t => t.Value is T).Value;
        }
    }
}
