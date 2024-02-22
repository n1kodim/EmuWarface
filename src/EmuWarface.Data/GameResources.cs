using EmuWarface.Common;
using EmuWarface.Data.Settings.GameConfiguration;
using EmuWarface.Server.Data.Settings;
using NLog;
using System.Xml.Linq;

namespace EmuWarface.Server.Data
{
    public class GameResources
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private const string ResourcesPath = "resources";

        public GameConfigurationSettings GameConfigSettings { get; private set; }
        public ExperienceCurveSettings ExpCurveSettings { get; private set; }

        public GameResources() 
        {
            // TODO: handle if any of .xml not exists

            var el1 = XElement.Load(Path.Combine(ResourcesPath, "game_configuration.xml"));
            var el2 = XElement.Load(Path.Combine(ResourcesPath, "exp_curve.xml"));

            GameConfigSettings = el1.FromXElement<GameConfigurationSettings>();
            ExpCurveSettings = new ExperienceCurveSettings(el2);

            _logger.Info("Game version: {0}", GameConfigSettings.GameVersion);
            _logger.Info("Loaded {0} rank(s)", ExpCurveSettings.ExpCurve.Count);
        }
    }
}
