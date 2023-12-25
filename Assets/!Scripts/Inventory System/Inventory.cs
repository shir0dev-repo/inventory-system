namespace Shir0.InventorySystem
{
    /// <summary>
    /// Base class for a collection of <see cref="InventorySlot"/>, 
    /// which acts as both a wrapper and a handler for adding and removing items.
    /// </summary>
    [System.Serializable]
    public class Inventory
    {
        [UnityEngine.SerializeField] private InventorySlot[] m_slots;
        [UnityEngine.SerializeField] private int m_size;

        /// <summary>
        /// Inventory slots contained within this Inventory.
        /// </summary>
        public InventorySlot[] Slots
        {
            get { return m_slots; }
            private set { m_slots = value; }
        }
        /// <summary>
        /// Size of the inventory, in terms of slot count.
        /// </summary>
        public int Size
        {
            get { return m_size; }
            private set { m_size = value; }
        }

        /// <summary>
        /// Creates a new Inventory with a specified size.
        /// </summary>
        /// <param name="size">Size of the new inventory.</param>
        public Inventory(int size)
        {
            if (size <= 0)
                throw new InvalidInventoryOperationException("Inventory cannot be size zero!");

            m_size = size;
            m_slots = new InventorySlot[size];
        }
        /// <summary>
        /// Creates a new Inventory that matches another.
        /// </summary>
        /// <param name="slots">Inventory slot array to copy from.</param>
        public Inventory(InventorySlot[] slots)
        {
            m_slots = slots;
            m_size = slots.Length;
        }
        /// <summary>
        /// Adds a specified amount of items to the inventory.
        /// </summary>
        /// <param name="itemToAdd">The item to add.</param>
        /// <param name="amount">The amount being added.</param>
        /// <returns>If the item was successfully added to the inventory.</returns>
        /// <exception cref="InvalidInventoryOperationException">Throws if item is null or amount is less than 1.</exception>
        public bool AddItem(ItemData itemToAdd, int amount)
        {
            if (amount < 1)
                throw new InvalidInventoryOperationException("Cannot add to inventory with zero or less items!");
            else if (itemToAdd == null)
                throw new InvalidInventoryOperationException("Cannot add null item to inventory slot. Try using InventorySlot.AssignItem(ItemData, int).");

            foreach (InventorySlot slot in m_slots)
            {
                if (slot.AddToStack(itemToAdd, amount, out int remainder) && remainder == 0)
                    return true;
                else if (remainder > 0)
                {
                    amount = remainder;
                    continue;
                }
            }

            return false;
        }

        public int SlotsRemaining()
        {
            int emptySlotCount = 0;
            foreach (InventorySlot slot in m_slots)
            {
                if (slot.Item == null)
                    emptySlotCount++;
            }

            return emptySlotCount;
        }
    }
}