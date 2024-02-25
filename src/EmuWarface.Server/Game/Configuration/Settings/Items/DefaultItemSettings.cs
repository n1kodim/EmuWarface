using EmuWarface.Server.Game.Channel;
using System.Xml.Serialization;

namespace EmuWarface.Server.Game.Configuration.Settings.Items
{
    public class DefaultItemSettings
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        //[XmlIgnore]
        //public object AllowedClasses { get; set; }
        //[XmlAttribute("classes")]
        //public string _allowedClasses
        //{
        //    get { return Channel.ToString(); }
        //    set { Channel = value.ToEnum<ChannelType>(); }
        //}
    }
}
