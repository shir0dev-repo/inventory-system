using System.Collections.Generic;

namespace Shir0.InventorySystem
{
    /// <summary>
    /// A collection of <see cref="InventorySlot"/> with a specified <see cref="Size"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="Inventory"/> is an <see cref="InventorySlot"/> collection in charge of keeping track of held item data.<br/>
    /// 
    /// </remarks>
    [System.Serializable]
    public class Inventory
    {
        private readonly int m_size;
        [UnityEngine.SerializeField] private InventorySlot[] m_slots;

        /// <summary>
        /// <see cref="InventorySlot"/>[] captured within this Inventory.
        /// </summary>
        /// <remarks><see langword="readonly"/> getter for <see cref="m_slots"/>.</remarks>
        public InventorySlot[] Slots
        {
            get { return m_slots; }
            private set { m_slots = value; }
        }

        /// <summary>
        /// Size of <see cref="Slots"/> <see cref="InventorySlot"/>[].
        /// </summary>
        /// <remarks><see langword="readonly"/> getter for <see cref="m_size"/>.</remarks>
        public int Size
        {
            get { return m_size; }
        }

        /// <summary>
        /// A count of <see cref="Slots"/> where the <see cref="InventorySlot"/> has no assigned <see cref="ItemData"/>.
        /// </summary>
        /// <returns>The amount of <see cref="InventorySlot"/> in <see cref="Slots"/> that have no assigned <see cref="ItemData"/>.</returns>
        public int SlotsRemaining
        {
            get
            {
                int slotCount = 0;
                foreach (InventorySlot slot in m_slots)
                {
                    if (slot.CurrentItem == null)
                        slotCount++;
                }

                return slotCount;
            }
        }

        /// <summary>
        /// Is every <see cref="InventorySlot"/> inside the <see cref="Inventory"/> full?
        /// </summary>
        /// <remarks>
        /// <see langword="true"/>: Every slot in <see cref="Slots"/> has an item.<br/>
        /// <see langword="false"/>: At least one slot in <see cref="Slots"/> has no item.
        /// </remarks>
        public bool IsFull
        {
            get
            {
                return SlotsRemaining < 1;
            }
        }

        /// <summary>
        /// Default constructor specifying the <paramref name="size"/> of <see cref="Slots"/>.
        /// </summary>
        /// <param name="size">Size of the <see cref="Inventory"/>.</param>
        /// <exception cref="InvalidInventoryOperationException"></exception>
        public Inventory(int size)
        {
            if (size < 1)
                throw new InvalidInventoryOperationException("Inventory cannot be size zero!");

            m_size = size;
            m_slots = new InventorySlot[size];

            for (int i = 0; i < size; i++)
                m_slots[i] = new InventorySlot();
            
        }

        /// <summary>
        /// Constructor for copying one <see cref="Inventory"/> to a newly constructed one, using the supplied <see cref="InventorySlot"/>[].
        /// </summary>
        /// <remarks>This constructor is implemented to future-proof implementing <see cref="UnityEngine.JsonUtility"/> serialization/deserialization.</remarks>
        /// <param name="slotsOther"><see cref="InventorySlot"/>[] to copy from.</param>
        public Inventory(InventorySlot[] slotsOther)
        {
            m_slots = slotsOther;
            m_size = slotsOther.Length;
        }

        public InventorySlot this[int i]
        {
            get { return m_slots[i]; }
            private set { m_slots[i] = value; }
        }

        private bool TryAddItem(ItemData item, int amount, out InventorySlot[] targetSlots)
        {
            if (amount < 1)
                throw new InvalidInventoryOperationException("Cannot add to inventory with zero or less items!");
            else if (item == null)
                throw new ItemNotFoundException("Cannot add null item to inventory slot! Try using InventorySlot.ClearSlot().");
            else if (IsFull) // no slots available, could not add any items to inventory.
            {
                targetSlots = null;
                return false;
            }

            List<InventorySlot> tSlots = new();

            InventorySlot[] availableSlots = FindMatchingSlots(item); // array containing all valid slots to add to.

            foreach (InventorySlot slot in availableSlots)
            {
                if (slot.EnoughRoomInStack(amount, out int remaining) || remaining > 0)
                {
                    tSlots.Add(slot);
                }
            }
            targetSlots = tSlots.ToArray();
            return tSlots.Count > 0;
        }

        /// <summary>
        /// Adds a specified <paramref name="amount"/> of <see cref="ItemData"/> to the <see cref="Inventory"/>.
        /// </summary>
        /// <remarks>
        /// Before attempting to add <paramref name="amount"/> of <paramref name="itemToAdd"/> to <see cref="Slots"/>, check for an <paramref name="amount"/>
        /// &lt; zero or a <see langword="null"/> <paramref name="itemToAdd"/>.<br/>
        /// If either of these are true, <see langword="throw"/> an <see cref="InvalidInventoryOperationException"/> or <see cref="ItemNotFoundException"/> respectively.
        /// <br/><br/>
        /// <see cref="AddItem(ItemData, int)"/> creates a cloned <see cref="InventorySlot"/>[] from <see cref="Slots"/>, only containing elements<br/>
        /// which match the given <paramref name="itemToAdd"/> or are empty, prioritizing matching slots first. It then iterates through each slot<br/>
        /// from this array, trying to add the specified <see cref="ItemData"/> to the <see cref="InventorySlot.CurrentStackSize"/>. If it successfully<br/>
        /// adds <paramref name="amount"/> to the stack, <see langword="return true"/>. Otherwise, assign the remaining value from 
        /// <see cref="InventorySlot.AddToStack(ItemData, int, out int)"/><br/> to <paramref name="amount"/> and continue iterating. If <paramref name="amount"/>
        /// does not reach zero by the end of the iteration, <see langword="return false"/>.
        /// <br/><br/>
        /// Currently, <see cref="AddItem(ItemData, int)"/> does not handle cases where some, but not all of <paramref name="amount"/> is added to the 
        /// <see cref="Inventory"/>,<br/> which will cause the remainder to be deleted or ignored when exiting the method.
        /// </remarks>
        /// <param name="itemToAdd">The item to add.</param>
        /// <param name="amount">The amount being added.</param>
        /// <returns>Was <paramref name="itemToAdd"/> successfully added to the <see cref="Inventory"/>?</returns>
        /// <exception cref="InvalidInventoryOperationException"></exception>
        /// <exception cref="ItemNotFoundException"></exception>
        public bool AddItem(ItemData itemToAdd, int amount)
        {
            if (TryAddItem(itemToAdd, amount, out InventorySlot[] targetSlots))
            {
                foreach (InventorySlot slot in targetSlots)
                {
                    slot.AddToStack(itemToAdd, amount, out int remainder);
                    if (remainder > 0)
                    {
                        UnityEngine.Debug.Log(remainder);
                        amount = remainder;
                        continue;
                    }
                    else return true;
                }
            }

            return false; // could not add any items to inventory.
        }

        /// <summary>
        /// Removes a specified <paramref name="amount"/> of <paramref name="itemToRemove"/> from the <see cref="Inventory"/>.
        /// </summary>
        /// <remarks>
        /// Before attempting to remove <paramref name="amount"/> of <paramref name="itemToRemove"/> to <see cref="Slots"/>, check for an <paramref name="amount"/>
        /// &lt; zero or a <see langword="null"/> <paramref name="itemToRemove"/>.<br/>
        /// If either of these are true, <see langword="throw"/> an <see cref="InvalidInventoryOperationException"/> or <see cref="ItemNotFoundException"/> respectively.
        /// <br/><br/>
        /// <see cref="RemoveItem(ItemData, int)"/> first uses <see cref="FindMatchingSlots(ItemData, bool)"/> to create a cloned <see cref="InventorySlot"/>[]<br/>
        /// from <see cref="Slots"/>, only containing elements which match the given <paramref name="itemToRemove"/>. It then iterates through each slot<br/>
        /// from this array, removing the specified <see cref="ItemData"/> from the <see cref="InventorySlot.CurrentStackSize"/>. If it successfully removes the full<br/>
        /// <paramref name="amount"/> from the slot's stack, call <see cref="InventorySlot.ClearSlot"/> and <see langword="return true"/>.<br/>
        /// Otherwise, assign the remaining value from <see cref="InventorySlot.RemoveFromStack(int, out int)"/> to <paramref name="amount"/> and continue iterating.<br/>
        /// If <paramref name="amount"/> does not reach zero by the end of the iteration, <see langword="return false"/>.
        /// <br/><br/>
        /// Currently, <see cref="RemoveItem(ItemData, int)"/> does not handle cases where some, but not all <paramref name="itemToRemove"/> can be removed.<br/>
        /// If the enclosing scope requires <see cref="RemoveItem(ItemData, int)"/> to be <see langword="true"/> to continue functionality, this will cause the <br/>
        /// <see cref="Inventory"/> to delete the items without the desired outcome.
        /// </remarks>
        /// <param name="itemToRemove">The target <see cref="ItemData"/> to remove from the <see cref="Inventory"/>.</param>
        /// <param name="amount">The amount of <see cref="ItemData"/> to remove from the <see cref="Inventory"/>.</param>
        /// <returns></returns>
        /// <exception cref="ItemNotFoundException"></exception>
        /// <exception cref="InvalidInventoryOperationException"></exception>
        public bool RemoveItem(ItemData itemToRemove, int amount)
        {
            if (amount < 1)
                throw new InvalidInventoryOperationException("Cannot remove zero items!");

            else if (itemToRemove == null)
                throw new ItemNotFoundException("Cannot remove null item from inventory!");


            InventorySlot[] matchingSlots = FindMatchingSlots(itemToRemove); // find all slots that have itemToRemove.
            foreach (InventorySlot slot in matchingSlots)
            {
                if (slot.CurrentItem == null) continue; // ignore empty slots returned by GetAvailableSlots.
                if (slot.CurrentItem == itemToRemove) // found the itemToRemove.
                {
                    // try removing amount of itemToRemove from current slot. If all items requested were removed, return true.
                    if (slot.RemoveFromStack(amount, out int remainingRequired))
                    {
                        return true;
                    }
                    else // not all items were successfully removed from this slot. Assign the remaining amount and iterate through.
                    {
                        amount = remainingRequired;
                        continue;
                    }
                }
            }

            return false; // if we were unable to break from the loop during slot.RemoveFromStack, we could not remove all items.
        }

        /// <summary>
        /// Gets every <see cref="InventorySlot"/> in <see cref="Slots"/> that either contains the specified <paramref name="item"/>, or is empty.<br/>
        /// if <paramref name="ignoreEmpty"/> is <see langword="true"/>, will only return matching slots.
        /// </summary>
        /// <remarks>
        /// Before iterating, check that <c><paramref name="item"/> != <see langword="null"/></c>. If <see langword="true"/>, <see langword="throw"/> an
        /// <see cref="ItemNotFoundException"/>.
        /// <br/><br/>
        /// Create two collections of <see cref="InventorySlot"/>, and iterate through <see cref="Slots"/>.<br/>
        /// For each <see cref="InventorySlot"/> which contains <paramref name="item"/>, add it to the matching item collection.<br/>
        /// If <paramref name="ignoreEmpty"/> is <see langword="false"/>, and the 
        /// current <see cref="InventorySlot"/> has no assigned <see cref="ItemData"/>,<br/>
        /// add it to the collection of <see cref="InventorySlot"/> with unassigned <see cref="ItemData"/>.
        /// <br/><br/>
        /// Post-iteration, if <paramref name="ignoreEmpty"/> is <see langword="false"/>, append the <see cref="InventorySlot"/>[]
        /// with no assigned <see cref="ItemData"/><br/>
        /// to the <see cref="InventorySlot"/>[] that matches the specified <paramref name="item"/>,
        /// which allows existing <see cref="ItemData"/> to be <br/>
        /// filled first and prevents <see cref="Inventory"/> clogging.
        /// </remarks>
        /// <param name="item">item to check against.</param>
        /// <param name="ignoreEmpty">Only include <see cref="InventorySlot"/> that directly match <paramref name="item"/>?</param>
        /// <returns><see cref="InventorySlot"/>[] containing <paramref name="item"/>. If <paramref name="ignoreEmpty"/> is false, appends
        /// <see cref="InventorySlot"/>[] with no assigned <see cref="ItemData"/>.</returns>
        /// <exception cref="ItemNotFoundException"></exception>
        InventorySlot[] FindMatchingSlots(ItemData item, bool ignoreEmpty = false)
        {
            if (item == null)
                throw new ItemNotFoundException("Cannot iterate with null item!");

            List<InventorySlot> matchingSlots = new();
            List<InventorySlot> emptySlots = new();

            foreach (InventorySlot slot in m_slots)
            {
                if (!ignoreEmpty && slot.CurrentItem == null)
                    emptySlots.Add(slot);
                else if (slot.CurrentItem == item)
                    matchingSlots.Add(slot);
            }
            if (!ignoreEmpty)
                matchingSlots.AddRange(emptySlots);

            return matchingSlots.ToArray();
        }
    }
}