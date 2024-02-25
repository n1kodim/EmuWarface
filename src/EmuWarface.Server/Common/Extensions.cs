using NLog;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EmuWarface.Common
{
    public static class Extensions
    {
        public static void Xmpp(this Logger logger, string data, string ip, bool isReceive)
        {
            logger
                .WithProperty("type", isReceive ? "RECV" : "SENT")
                .WithProperty("ip", ip)
                .Debug(data);
        }

        public static XElement ToXElement<T>(this object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(streamWriter, obj);
                    return XElement.Parse(Encoding.ASCII.GetString(memoryStream.ToArray()));
                }
            }
        }

        public static T FromXElement<T>(this XElement xElement)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            return (T)xmlSerializer.Deserialize(xElement.CreateReader());
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
