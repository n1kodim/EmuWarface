using EmuWarface.Common;
using MiniXML;

namespace EmuWarface.Server.Game.Channel
{
    public class MasterServer
    {
        public int Id { get; private set; }
        public int MinRank { get; private set; }
        public int MaxRank { get; private set; }
        public int MaxUsers { get; private set; }
        public ChannelType Type { get; private set; }
        public ChannelRankGroup RankGroup { get; private set; }
        public string Bootstrap { get; private set; }
        public int Online { get; private set; }
        public float Load
        {
            get => (float)Online / MaxUsers;
        }
        public Jid Jid { get; private set; }
        public string Resource
        {
            get => Jid.Resource;
        }

        public MasterServer(int id, ChannelType type, int maxUsers, int minRank, int maxRank)
        {
            Id = id;
            MinRank = minRank;
            MaxRank = maxRank;
            MaxUsers = maxUsers;
            Type = type;
            RankGroup = ChannelRankGroup.All; // official server always sends rank_group='all'
            Bootstrap = string.Empty;

            var resource = Type.GetName().ToLower() + "_" + Id.ToString("000");
            Jid = new Jid(Globals.MasterServerNode, Globals.XmppDomain, resource);
        }

        public Element Serialize()
        {
            return new Element("server")
                .Attr("resource", Resource)
                .Attr("server_id", Id)
                .Attr("channel", Type.GetName().ToLower())
                .Attr("rank_group", RankGroup.GetName().ToLower())
                .Attr("load", (Load, "F3"))
                .Attr("online", Online)
                .Attr("min_rank", MinRank)
                .Attr("max_rank", MaxRank)
                .Attr("bootstrap", Bootstrap)
                .C(new Element("load_stats")
                .C(new Element("load_stat").Attr("type", "quick_play").Attr("value", "255"))
                .C(new Element("load_stat").Attr("type", "survival").Attr("value", "255"))
                .C(new Element("load_stat").Attr("type", "pve").Attr("value", "255")));
        }
    }
}
