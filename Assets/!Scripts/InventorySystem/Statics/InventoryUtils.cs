using System.Collections.Generic;

namespace Shir0.InventorySystem
{
    /// <summary>
    /// A collection of static utilities for an inventory.
    /// </summary>
    public static class InventoryUtils
    {
        /// <summary>
        /// Finds all slots in the inventory that contain the specified item.
        /// </summary>
        /// <param name="inventory">The inventory to search.</param>
        /// <param name="item">Item to search for.</param>
        /// <returns>An array of matching indices for every <see cref="InventorySlot"/> containing the item.</returns>
        /// <exception cref="InvalidInventoryOperationException"/>
        public static int[] FindMatchingSlotIndices(this Inventory inventory, ItemData item)
        {
            if (item == null)
                throw new InvalidInventoryOperationException("Cannot iterate with null item!");

            List<int> matchingIndices = new();
            for (int i = 0; i < inventory.Slots.Length; i++)
            {
                if (inventory.Slots[i].HasItem && inventory.Slots[i].CurrentItem == item)
                    matchingIndices.Add(i);
            }

            return matchingIndices.ToArray();
        }

        /// <summary>
        /// Finds all slots in the inventory that can hold a specified item.
        /// </summary>
        /// <param name="inventory">The inventory to search.</param>
        /// <param name="item">The item to search with.</param>
        /// <returns>An array of slots; first matching, then empty.</returns>
        public static int[] FindValidSlotIndices(this Inventory inventory, ItemData item)
        {
            if (item == null)
                throw new InvalidInventoryOperationException("Cannot iterate with null item!");

            List<int> matchingIndices = new List<int>();
            List<int> emptyIndices = new List<int>();

            for (int i = 0; i < inventory.Slots.Length; i++)
            {
                if (!inventory.Slots[i].HasItem)
                    emptyIndices.Add(i);
                else if (inventory.Slots[i].HasItem && inventory.Slots[i].CurrentItem == item)
                    matchingIndices.Add(i);
            }

            matchingIndices.AddRange(emptyIndices);
            return matchingIndices.ToArray();
        }
    }
}
