namespace EmuWarface.Server.Game.Player
{
    [Flags]
    public enum PlayerStatus
    {
        Offline                     = 0,
        Online                      = 1 << 0,
        Logout                      = 1 << 1,
        Away                        = 1 << 2,
        InLobby                     = 1 << 3,
        InGameRoom                  = 1 << 4,
        InGame                      = 1 << 5,
        InShop                      = 1 << 6,
        InCustomize                 = 1 << 7,
        InRatingGame                = 1 << 8,
        InTutorialGame              = 1 << 9,
        BannedInRatingGame          = 1 << 10,
        BannedInPvpAutostartGame    = 1 << 11
    }
}
