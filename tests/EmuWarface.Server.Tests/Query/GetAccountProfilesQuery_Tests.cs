using EmuWarface.Server.Common.Configuration;
using EmuWarface.Server.Game.Player;
using EmuWarface.Server.Query.Handlers;
using Xunit.DependencyInjection;

namespace EmuWarface.Server.Tests.Query
{
    public class GetAccountProfilesQuery_Tests
    {
        private readonly ITestOutputHelperAccessor _testOutputHelperAccessor;
        private readonly GetAccountProfilesQuery _handler;
        private readonly GameOptions _gameOptions;
        private readonly MockPlayer _player;

        public GetAccountProfilesQuery_Tests(ITestOutputHelperAccessor testOutputHelperAccessor, GetAccountProfilesQuery queryHandler, GameOptions gameOptions)
        {
            _testOutputHelperAccessor = testOutputHelperAccessor;
            _gameOptions = gameOptions;
            _handler = queryHandler;
            _player = new MockPlayer("1");
        }

        [Fact]
        public async void Invalid_GameVersion_ReturnsError()
        {
            var q = $"<get_account_profiles version='1.2.3.4' user_id='{_player.Jid.Local}' token=' '/>".ToXml();
            var res = await _handler.HandleRequestAsync(_player, q);

            Assert.Equal(1, res.Error);
        }

        [Fact]
        public async void Invalid_UserId_ReturnsError()
        {
            var q = $"<get_account_profiles version='{_gameOptions.Version}' user_id='666' token=' '/>".ToXml();
            var res = await _handler.HandleRequestAsync(_player, q);

            Assert.Equal(2, res.Error);
        }

        [Fact]
        public async void CorrectArgs_ReturnsElement()
        {
            var q = $"<get_account_profiles version='{_gameOptions.Version}' user_id='{_player.Jid.Local}' token=' '/>".ToXml();
            var res = await _handler.HandleRequestAsync(_player, q);

            Assert.Equal(0, res.Error);

            _testOutputHelperAccessor.Output.WriteLine(res.Value.ToString());
        }
    }
}
