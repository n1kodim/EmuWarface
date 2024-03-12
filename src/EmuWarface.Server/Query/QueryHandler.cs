using EmuWarface.Server.Common;
using EmuWarface.Server.Game.Player;
using MiniXML;

namespace EmuWarface.Server.Query
{
    public abstract class QueryHandler
    {
        public virtual Task<Result<Element?, int>> HandleRequestAsync(IOnlinePlayer player, Element query)
        {
            throw new NotImplementedException();
        }

        public virtual Task<int> HandleResponseAsync(IOnlinePlayer player, Element query)
        {
            throw new NotImplementedException();
        }
    }
}
