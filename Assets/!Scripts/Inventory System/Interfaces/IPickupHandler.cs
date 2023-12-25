namespace Shir0.InventorySystem
{
    public interface IPickupHandler
    {
        void HandleItemPickup(ItemData item, int amount);
        void DropItem(ref InventorySlot slot, int amount = -1);
    }
}