using EmuWarface.Server.Common.Attributes;
using EmuWarface.Server.Game.Data.Configuration;
using EmuWarface.Server.Game.Data.Configuration.Masterserver;
using NLog;

namespace EmuWarface.Server.Game.Channel
{
    [Service]
    public class ChannelManager
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly MasterServerConfig _masterserverConfig;
        private readonly ExperienceCurveConfig _experienceCurveConfig;
        private readonly List<MasterServer> _masterServers;

        public ChannelManager(MasterServerConfig masterserverConfig, ExperienceCurveConfig experienceCurveConfig)
        {
            _masterserverConfig = masterserverConfig;
            _experienceCurveConfig = experienceCurveConfig;
            _masterServers = new List<MasterServer>();
        }

        public void InitChannels()
        {
            var channelIds = new Dictionary<ChannelType, int>();
            foreach (var server in _masterserverConfig.ChannelSettings)
            {
                if (!server.Enabled)
                    continue;

                int minRank = 0;
                int maxRank = _experienceCurveConfig.GlobalMaxRank;

                var restriction = _masterserverConfig.ChannelRestrictions
                    .FirstOrDefault(x => x.Channel == server.Channel);

                if (restriction != null)
                {
                    minRank = restriction.MinRank;
                    maxRank = restriction.MaxRank != 0 ? restriction.MaxRank : maxRank;
                }

                // increment channel id
                if (!channelIds.ContainsKey(server.Channel))
                    channelIds[server.Channel] = 0;

                channelIds[server.Channel]++;
                var serverId = (int)server.Channel * 100 + channelIds[server.Channel];

                var masterserver = new MasterServer(serverId, server.Channel, server.MaxUsers, minRank, maxRank);
                _masterServers.Add(masterserver);
            }

            _logger.Info("Initialised {0} channel(s)", _masterServers.Count);
            _logger.Debug(string.Join(", ", _masterServers.Select(x => x.Resource).ToArray()));
        }

        public IEnumerable<MasterServer> GetChannels()
        {
            return _masterServers;
        }

        public MasterServer? GetChannel(ChannelType channel)
        {
            return _masterServers.FirstOrDefault(x => x.Type == channel && x.Online < x.MaxUsers);
        }
    }
}
