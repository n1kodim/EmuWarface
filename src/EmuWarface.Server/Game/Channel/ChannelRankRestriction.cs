namespace EmuWarface.Server.Game.Channel
{
    public class ChannelRankRestriction
    {
        public ChannelRankRestriction(uint channelMinRank, uint channelMaxRank)
        {
            this.MinRank = channelMinRank;
            this.MaxRank = channelMaxRank;
        }

        public uint MinRank { get; private set; }
        public uint MaxRank { get; private set; }
    }
}
