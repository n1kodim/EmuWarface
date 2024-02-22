using EmuWarface.Common.Enums;
using EmuWarface.Common;
using System.Xml.Serialization;

namespace EmuWarface.Data.Settings.GameConfiguration
{
    public class ChannelRestrictionSettings
    {
        [XmlIgnore]
        public ChannelType Channel { get; set; }
        [XmlAttribute("channel")]
        public string _channel
        {
            get { return Channel.ToString(); }
            set { Channel = value.ToEnum<ChannelType>(); }
        }

        [XmlAttribute("min_rank")]
        public int MinRank { get; set; }

        [XmlAttribute("max_rank")]
        public int MaxRank { get; set; }
    }
}
