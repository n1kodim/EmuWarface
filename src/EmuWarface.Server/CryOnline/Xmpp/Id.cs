namespace EmuWarface.Server.CryOnline.Xmpp
{
    public class Id
    {
        private static long _id;
        public static string Prefix { get; set; } = "uid";
        public static string GetNextId() => Prefix + Interlocked.Increment(ref _id).ToString("x8");
        public static void Reset()
        {
            _id = 0;
        }
    }
}
