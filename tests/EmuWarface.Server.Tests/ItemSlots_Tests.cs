using EmuWarface.Server.Game.Item;

namespace EmuWarface.Server.Tests
{
    public class ItemSlots_Tests
    {
        [Fact]
        public async void TestSlot()
        {
            ulong attachment20 = 21647380;
            Assert.Equal(20UL, SlotData.GetSlotId(attachment20, 0));
            Assert.Equal(20UL, SlotData.GetSlotId(attachment20, 2));
            Assert.Equal(20UL, SlotData.GetSlotId(attachment20, 3));
            Assert.Equal(20UL, SlotData.GetSlotId(attachment20, 4));

            ulong slotIds = 0;
            for(int i = 0; i < 5; i++)
            {
                if (i == 1)
                    continue;

                slotIds |= SlotData.SetSlotId(20, i);
            }

            Assert.Equal(21647380UL, slotIds);
            Assert.Equal(20UL, SlotData.GetSlotId(slotIds, 0));
            Assert.Equal(20UL, SlotData.GetSlotId(slotIds, 2));
            Assert.Equal(20UL, SlotData.GetSlotId(slotIds, 3));
            Assert.Equal(20UL, SlotData.GetSlotId(slotIds, 4));

            Assert.Equal(29UL, SlotData.ConvertSlotIdsToEquipped(slotIds));
        }
    }
}
