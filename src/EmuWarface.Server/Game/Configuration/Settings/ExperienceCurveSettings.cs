using System.Xml.Linq;

namespace EmuWarface.Server.Game.Configuration.Settings
{
    public class ExperienceCurveSettings
    {
        public ExperienceCurveSettings(XElement e)
        {
            ExpCurve = new List<ulong>();

            foreach (var item in e.Elements())
            {
                if (!item.Name.LocalName.StartsWith("level"))
                    continue;

                ulong exp = ulong.Parse(item.Attribute("exp").Value);
                ExpCurve.Add(exp);
            }

            ExpCurve.Sort();
        }

        public List<ulong> ExpCurve { get; set; }
        public int GlobalMaxRank => ExpCurve.Count;
    }
}
