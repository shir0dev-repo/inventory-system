using System;

namespace Shir0.InventorySystem
{
    /// <summary>
    /// Holds a dynamically sized stack of <see cref="ItemData"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="InventorySlot"/>s are intended, but not required, to be used in conjunction with <see cref="Inventory"/> as an element of its collection.<br/>
    /// <see cref="InventorySlot"/> handles keeping track of its assigned <see cref="ItemData"/>, and its corresponding amount.<br/><br/>
    /// </remarks>
    /// 
    [Serializable]
    public class InventorySlot
    {
        [UnityEngine.SerializeField] private ItemData m_item;
        [UnityEngine.SerializeField] private int m_currentStackSize = -1;

        /// <summary>
        /// The current <see cref="ItemData"/> this slot holds.
        /// </summary>
        public ItemData CurrentItem
        {
            get { return m_item; }
        }

        /// <summary>
        /// The amount of <see cref="CurrentItem"/> this slot holds.
        /// </summary>
        public int CurrentStackSize
        {
            get
            { return m_currentStackSize; }
        }
        /// <summary>
        /// <see cref="CurrentItem"/> <see langword="is not null"/>.
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
                if (m_currentStackSize == m_item.MaxStackSize) // no space, can't add items.
                {
                    remainder = amount;
                    return false;
                }
                else if (m_currentStackSize + amount <= m_item.MaxStackSize) // enough space, add amount to stack.
                {
                    m_currentStackSize += amount;
                    remainder = 0;
                    return true;
                }
                else // not enough space, fill the slot and assign remainder.
                {
                    remainder = m_currentStackSize + amount - m_item.MaxStackSize;
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
        /// Removes a specified <paramref name="amount"/> of items from slot.
        /// </summary>
        /// <param name="amount">Amount of <see cref="ItemData"/> to remove from slot.</param>
        /// <param name="remainingRequired">
        ///     if <paramref name="amount"/> is greater than current stack size, 
        ///     <paramref name="remainingRequired"/> is the remainder after the slot is emptied.<br/>
        ///     otherwise, <paramref name="remainingRequired"/> is zero.
        /// </param>
        /// <returns>
        /// <see langword="true"/>: <paramref name="amount"/> was successfully removed from slot.<br/>
        /// <see langword="false"/>: more items are required.
        /// </returns>
        public bool RemoveFromStack(int amount, out int remainingRequired)
        {
            if (m_currentStackSize - amount < 0) // stack size cannot support amount of items removed.
            {
                remainingRequired = -(m_currentStackSize - amount);
                ClearSlot();
                return false;
            }
            else // enough items to match amount requested.
            {
                remainingRequired = 0;
                m_currentStackSize -= amount;
                if (m_currentStackSize < 1)
                    ClearSlot();

                return true;
            }
        }

        public bool EnoughRoomInStack(int amount, out int remaining)
        {
            if (m_item == null)
            {
                remaining = 0;
                return true;
            }

            if (m_item.MaxStackSize - m_currentStackSize - amount >= 0)
            {
                remaining = 0;
                return true;
            }
            else
            {
                remaining = (int)MathF.Abs(m_item.MaxStackSize - m_currentStackSize - amount);
                return false;
            }
        }

        public bool EnoughItemsInStack(int amount, out int remainingNeeded)
        {
            if (m_item == null)
            {
                remainingNeeded = amount;
                return false;
            }
            if (m_currentStackSize - amount >= 0)
            {
                remainingNeeded = 0;
                return true;
            }
            else
            {
                remainingNeeded = (int)MathF.Abs(m_currentStackSize - amount);
                return false;
            }
        }
    }
}