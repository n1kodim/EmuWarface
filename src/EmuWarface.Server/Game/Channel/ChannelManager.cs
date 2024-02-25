using EmuWarface.Server.Game.Configuration;
using NLog;

namespace EmuWarface.Server.Game.Channel
{
    public class ChannelManager
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private GameResources _resourceManager;
        private readonly List<MasterServer> _masterServers;

        public ChannelManager(GameResources resourceManager)
        {
            _resourceManager = resourceManager;
            _masterServers = new List<MasterServer>();

            foreach (var server in _resourceManager.GameConfig.MasterServers)
            {
                if (!server.Enabled)
                    continue;

                int minRank = 0;
                int maxRank = _resourceManager.ExpCurve.GlobalMaxRank;

                var restriction = _resourceManager.GameConfig.ChannelRestrictions
                    .FirstOrDefault(x => x.Channel == server.Channel);

                if (restriction != null)
                {
                    minRank = restriction.MinRank;
                    maxRank = restriction.MaxRank != 0 ? restriction.MaxRank : maxRank;
                }

                var masterserver = new MasterServer(server.Id, server.Channel, server.MaxUsers, minRank, maxRank);
                _masterServers.Add(masterserver);
            }

            _logger.Info("Initialised {0} channel(s): ({1})",
                _masterServers.Count,
                string.Join(",", _masterServers.Select(x => x.Resource).ToArray()));
        }

        public IEnumerable<MasterServer> GetChannels()
        {
            return _masterServers;
        }

        public MasterServer? GetChannel(ChannelType channel)
        {
            return _masterServers.FirstOrDefault(x => x.Channel == channel && x.Online < x.MaxUsers);
        }
    }
}
