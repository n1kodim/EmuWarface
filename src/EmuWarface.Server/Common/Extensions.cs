using NLog;

namespace EmuWarface.Common
{
    public static class Extensions
    {
        public static void Xmpp(this Logger logger, string data, string ip, bool isReceive)
        {
            logger
                .WithProperty("type", isReceive ? "RECV" : "SENT")
                .WithProperty("ip", ip)
                .Trace(data);
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string? GetName<T>(this T value) where T : Enum
        {
            return Enum.GetName(typeof(T), value);
        }
    }
}
