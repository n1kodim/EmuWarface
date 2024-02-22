using System.Xml.Serialization;

namespace EmuWarface.Data.Settings.GameConfiguration
{
    [XmlRoot("game_configuration")]
    public class GameConfigurationSettings
    {
        [XmlAttribute("game_version")]
        public string GameVersion { get; set; }

        [XmlArray("channel_restrictions")]
        [XmlArrayItem("restriction", Type = typeof(ChannelRestrictionSettings))]
        public ChannelRestrictionSettings[] ChannelRestrictions { get; set; }

        [XmlArray("masterservers")]
        [XmlArrayItem("server", Type = typeof(MasterServerSettings))]
        public MasterServerSettings[] MasterServers { get; set; }
    }
}
