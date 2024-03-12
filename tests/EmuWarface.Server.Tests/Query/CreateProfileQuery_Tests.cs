using Autofac;
using EmuWarface.DAL;
using EmuWarface.DAL.Repositories;
using EmuWarface.Server.Common.Configuration;
using EmuWarface.Server.Game;
using EmuWarface.Server.Game.Player;
using EmuWarface.Server.Game.Profile;
using EmuWarface.Server.Query.Handlers;
using MiniXML;
using Xunit.DependencyInjection;

namespace EmuWarface.Server.Tests.Query
{
    public class CreateProfileQuery_Tests
    {
        private readonly ITestOutputHelperAccessor _testOutputHelperAccessor;
        private readonly CreateProfileQuery _handler;
        private readonly GameOptions _gameOptions;
        private readonly MockPlayer _player;

        public CreateProfileQuery_Tests(ITestOutputHelperAccessor testOutputHelperAccessor, IComponentContext context/*, CreateProfileQuery queryHandler*/, GameOptions gameOptions)
        {
            _testOutputHelperAccessor = testOutputHelperAccessor;
            _gameOptions = gameOptions;
            ///_handler = queryHandler;
            var repos = new ProfileRepository(new GameDbContextFactory().CreateInMemoryDbContext());
            _handler = new CreateProfileQuery(context.Resolve<GameOptions>(), context.Resolve<ProfileManager>(), repos, context.Resolve<InputStringValidator>());
            _player = new MockPlayer("1");
        }

        [Fact]
        public async void Invalid_GameVersion_ReturnsError()
        {
            var q = GetQueryElement();

            q.Attr("user_id", _player.Jid.Local);
            q.Attr("version", _gameOptions.Version);
            q.Attr("nickname", "_-123456-_");
            q.Attr("head", "default_head_01");

            var res = await _handler.HandleRequestAsync(_player, q);

            Assert.Equal(0, res.Error);
            _testOutputHelperAccessor.Output.WriteLine(res.Value.ToString());
        }

        private static Element GetQueryElement()
        {
            return new Element("create_profile")
                .Attrs(new
                {
                    user_id = "1",
                    version = "1",
                    token = " ",
                    nickname = "1",
                    region_id = "global",
                    head = "default_head_01",
                    resource = "pvp_newbie_1",
                    hw_id = "101101101",
                    cpu_vendor = "1",
                    cpu_family = "6",
                    cpu_model = "10",
                    cpu_stepping = "7",
                    cpu_speed = "2291",
                    cpu_num_cores = "4",
                    gpu_vendor_id = "2318",
                    gpu_device_id = "1111",
                    physical_memory = "4079",
                    os_ver = "5",
                    os_64 = "1",
                    build_type = "--release"
                });
        }
    }
}
