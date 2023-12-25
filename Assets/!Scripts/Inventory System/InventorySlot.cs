namespace Shir0.InventorySystem
{
    /// <summary>
    /// Holds a dynamically sized stack of ItemData.
    /// </summary>
    [System.Serializable]
    public class InventorySlot
    {
        [UnityEngine.SerializeField] private ItemData m_item = null;
        [UnityEngine.SerializeField] private int m_currentStackSize = -1;
        /// <summary>
        /// The current ItemData occupying this slot.
        /// </summary>
        public ItemData Item
        {
            get { return m_item; }
            private set { m_item = value; }
        }
        /// <summary>
        /// The size of the current stack this slot holds.
        /// </summary>
        public int CurrentStackSize
        {
            get
            { return m_currentStackSize; }
            private set { m_currentStackSize = value; }
        }
        /// <summary>
        /// Checks if this slot is currently holding an item.
        /// </summary>
        public bool HasItem
        {
            get
            {
                return m_item != null;
            }
        }
        /// <summary>
        /// Returns whether or not this slot has reached maximum stack of items.
        /// </summary>
        public bool AtCapacity
        {
            get
            {
                if (m_item == null) 
                    return false;
                else 
                    return m_currentStackSize >= m_item.MaxStackSize;
            }
        }
        /// <summary>
        /// Creates a blank/empty inventory slot.
        /// </summary>
        public InventorySlot()
        {
            ClearSlot();
        }
        /// <summary>
        /// Creates an inventory slot using <see cref="AssignSlot(ItemData, int)"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="stackSize"></param>
        public InventorySlot(ItemData item, int stackSize)
        {
            AssignSlot(item, stackSize);
        }

        /// <summary>
        /// Clears this slot, assigning null and -1 to the item and stack size, respectively.
        /// </summary>
        public void ClearSlot()
        {
            m_item = null;
            m_currentStackSize = -1;
        }
        /// <summary>
        /// Forces this slot to have a specified item and stackSize.
        /// </summary>
        /// <param name="item">Item to assign.</param>
        /// <param name="stackSize">Amount to assign.</param>
        public void AssignSlot(ItemData item, int stackSize)
        {
            m_item = item;
            m_currentStackSize = stackSize;
        }
        /// <summary>
        /// Adds to this slot.
        /// </summary>
        /// <param name="itemToAdd">Given item to add to slot.</param>
        /// <param name="amount">Amount to add.</param>
        /// <param name="remainder">Amount remaining after adding to stack.</param>
        /// <returns></returns>
        public bool AddToStack(ItemData itemToAdd, int amount, out int remainder)
        {
            if (m_item != null && itemToAdd != m_item) // different items, can't add.
            {
                remainder = amount;
                return false;
            }
            else if (m_item == null) // empty slot, add item.
            {
                m_item = itemToAdd;
                m_currentStackSize = amount;
                remainder = 0;
                return true;
            }
            else if (m_item == itemToAdd) // same item, check stack sizes.
            {
                if (m_currentStackSize + amount <= m_item.MaxStackSize) // enough space, add amount to stack.
                {
                    m_currentStackSize += amount;
                    remainder = 0;
                    return true;
                }
                else // not enough space, fill the slot and assign remainder.
                {
                    remainder = (m_currentStackSize + amount) % m_item.MaxStackSize;
                    m_currentStackSize = m_item.MaxStackSize;
                    return true;
                }
            }
            else // no condition satisfied.
            {
                remainder = amount;
                return false;
            }
        }
        /// <summary>
        /// Removes from this slot.
        /// </summary>
        /// <param name="amount">Amount to remove.</param>
        public void RemoveFromStack(int amount)
        {
            m_currentStackSize -= amount;
            if (m_currentStackSize < 0)
                ClearSlot();
        }
    }
}