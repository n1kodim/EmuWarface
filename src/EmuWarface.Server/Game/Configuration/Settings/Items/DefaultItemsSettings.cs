using System.Xml.Serialization;

namespace EmuWarface.Server.Game.Configuration.Settings.Items
{
    [XmlRoot("items")]
    public class DefaultItemsSettings
    {
        [XmlElement("item", Type = typeof(DefaultItemSettings))]
        public DefaultItemSettings[] DefaultItems { get; set; }
    }
}
