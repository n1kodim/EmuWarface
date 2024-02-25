using EmuWarface.Common;
using EmuWarface.Server.Game.Channel;
using System.Xml.Serialization;

namespace EmuWarface.Server.Game.Configuration.Settings.GameConfiguration
{
    public class MasterServerSettings
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("max_users")]
        public int MaxUsers { get; set; }

        [XmlIgnore]
        public bool Enabled { get; set; }
        [XmlAttribute("enabled")]
        public string _enabled
        {
            get { return Enabled ? "1" : "0"; }
            set { Enabled = value == "1" || value == "true"; }
        }

        [XmlIgnore]
        public ChannelType Channel { get; set; }
        [XmlAttribute("channel")]
        public string _channel
        {
            get { return Channel.ToString(); }
            set { Channel = value.ToEnum<ChannelType>(); }
        }
    }
}
