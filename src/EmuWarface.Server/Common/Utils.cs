using System.Text;

namespace EmuWarface.Common
{
	public static class Utils
	{
		public static string EncodeBase64(string data) => Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
		public static string DecodeBase64(string data) => Encoding.UTF8.GetString(Convert.FromBase64String(data));

    }
}