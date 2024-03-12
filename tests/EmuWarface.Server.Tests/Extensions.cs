using MiniXML;
using System.Text;

namespace EmuWarface.Server.Tests;

public static class Extensions
{
    public static Element ToXml(this string str) => Xml.Parse(new MemoryStream(Encoding.UTF8.GetBytes(str)));
}