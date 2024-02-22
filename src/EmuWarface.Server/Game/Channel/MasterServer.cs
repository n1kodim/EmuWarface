using EmuWarface.Common.Enums;
using EmuWarface.Server.CryOnline.Xmpp;
using XmppDotNet;
using XmppDotNet.Xml;

namespace EmuWarface.Server.Game.Channel
{
    public class MasterServer : IXmppSerializable
    {
        public int Id { get; private set; }
        public int MinRank { get; private set; }
        public int MaxRank { get; private set; }
        public int MaxUsers { get; private set; }
        public ChannelType Channel { get; private set; }
        public ChannelRankGroup RankGroup { get; private set; }
        public string Bootstrap { get; private set; }
        public int Online { get; private set; }
        public double Load
        {
            get => (double)Online / MaxUsers;
        }
        public Jid Jid { get; private set; }
        public string Resource
        {
            get => Jid.Resource;
        }

        public MasterServer(int id, ChannelType channel, int maxUsers, int minRank, int maxRank)
        {
            Id = id;
            MinRank = minRank;
            MaxRank = maxRank;
            MaxUsers = maxUsers;
            Channel = channel;
            RankGroup = ChannelRankGroup.All; // official server always sends rank_group='all'
            Bootstrap = string.Empty;

            var resource = Channel.GetName().ToLower() + "_" + Id.ToString("000");
            Jid = new Jid(Globals.MasterServerNode, Globals.XmppDomain, resource);
        }

        public XmppXElement Serialize()
        {
            XmppXElement serverEl = new XmppXElement(Namespaces.CryOnline, "server");
            serverEl.SetAttribute("resource", Resource);
            serverEl.SetAttribute("server_id", Id);
            serverEl.SetAttribute("channel", Channel.GetName().ToLower());
            serverEl.SetAttribute("rank_group", RankGroup.GetName().ToLower());
            serverEl.SetAttribute("load", Load.ToString("F3").Replace(',', '.'));
            serverEl.SetAttribute("online", Online);
            serverEl.SetAttribute("min_rank", MinRank);
            serverEl.SetAttribute("max_rank", MaxRank);
            serverEl.SetAttribute("bootstrap", Bootstrap);

            var stats = new XmppXElement(Namespaces.CryOnline, "load_stats");
            stats.Add(new XmppXElement(Namespaces.CryOnline, "load_stat")
                .SetAttribute("type", "quick_play")
                .SetAttribute("value", "255"));
            stats.Add(new XmppXElement(Namespaces.CryOnline, "load_stat")
                .SetAttribute("type", "survival")
                .SetAttribute("value", "255"));
            stats.Add(new XmppXElement(Namespaces.CryOnline, "load_stat")
                .SetAttribute("type", "pve")
                .SetAttribute("value", "255"));

            serverEl.Add(stats);
            return serverEl;
        }
    }
}
