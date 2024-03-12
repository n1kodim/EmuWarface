using EmuWarface.Server.Game;

namespace EmuWarface.Server.Tests
{
    public class InputStringValidator_Tests
    {
        private readonly InputStringValidator _validator;

        public InputStringValidator_Tests(InputStringValidator validator) => _validator = validator;

        [Fact]
        public void ValidateNickname_Test()
        {
            var b1 = _validator.ValidateNickname("具有靜電產生裝置之影像輸入裝置", "Chinese");
            var b2 = _validator.ValidateNickname("具有_русский", "Chinese");
            var b3 = _validator.ValidateNickname("english_()nick.-", "English");
            var b4 = _validator.ValidateNickname("english生_()nick.-", "English");

            Assert.True(b1);
            Assert.False(b2);
            Assert.True(b3);
            Assert.False(b4);
        }
    }
}
