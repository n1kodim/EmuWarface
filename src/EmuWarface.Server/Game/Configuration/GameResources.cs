using EmuWarface.Common;
using EmuWarface.Server.Game.Configuration.Settings;
using EmuWarface.Server.Game.Configuration.Settings.GameConfiguration;
using EmuWarface.Server.Game.Configuration.Settings.Items;
using System.Xml.Linq;
using NLog;

namespace EmuWarface.Server.Game.Configuration
{
    public class GameResources
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private const string ResourcesPath = "Resources";

        public GameConfigurationSettings GameConfig { get; private set; }
        public DefaultItemsSettings DefaultItems { get; private set; }
        public ExperienceCurveSettings ExpCurve { get; private set; }

        public GameResources()
        {
            GameConfig = LoadConfiguration<GameConfigurationSettings>("game_configuration.xml");
            DefaultItems = LoadConfiguration<DefaultItemsSettings>("default_items.xml");
            ExpCurve = new ExperienceCurveSettings(LoadXmlConfiguration("exp_curve.xml"));

            _logger.Info("Game version: {0}", GameConfig.GameVersion);
            _logger.Info("Loaded {0} rank(s)", ExpCurve.ExpCurve.Count);
        }

        public T LoadConfiguration<T>(string filename)
        {
            // TODO: handle valid of Type
            return LoadXmlConfiguration(filename).FromXElement<T>();
        }

        public XElement LoadXmlConfiguration(string filename)
        {
            var path = Path.Combine(ResourcesPath, filename);
            return XElement.Load(path);
        }
    }
}
