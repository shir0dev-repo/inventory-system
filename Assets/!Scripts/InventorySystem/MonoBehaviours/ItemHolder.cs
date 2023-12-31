using System;
using UnityEngine;
namespace Shir0.InventorySystem
{
    /// <summary>
    /// <see cref="MonoBehaviour"/> housing an <see cref="Inventory"/>, along with its <see cref="Size"/> and other various data.
    /// </summary>
    /// <remarks>
    /// Instantiates a <see langword="new"/> <see cref="InventorySystem.Inventory"/> with the <see cref="Inventory.Size"/> set in the Inspector.<br/>
    /// The <see cref="ItemHolder"/> component is used as somewhat of a wrapper for the <see cref="InventorySystem.Inventory"/>, enabling it to access<br/>
    /// various <see cref="MonoBehaviour"/> methods. One such example is, along with the use of a <see cref="Rigidbody2D"/>, a player's<br/>
    /// <see cref="Rigidbody2D"/> can collide with an <see cref="ItemPickup"/>, and add the <see cref="ItemData"/> to its <see cref="InventorySystem.Inventory"/>.
    /// </remarks>
    public abstract class ItemHolder : MonoBehaviour
    {
        [SerializeField] protected Inventory m_inventory;
        [SerializeField] protected int m_size;
        [SerializeField] protected int m_addPriority = 0;

        public event EventHandler<EventArgs> OnInventoryDisplayRequested;

        /// <summary>
        /// The <see cref="InventorySystem.Inventory"/> assigned to this item holder.
        /// </summary>
        /// <remarks><see langword="throws"/> an <see cref="InventoryNotFoundException"/> when <see cref="Inventory"/> is <see langword="null"/>.</remarks>
        public Inventory Inventory
        {
            get
            {
                if (m_inventory == null)
                    throw new InventoryNotFoundException();
                else
                    return m_inventory;
            }
        }

        /// <summary>
        /// The size of this item holder's <see cref="InventorySystem.Inventory"/> set in the Inspector.
        /// </summary>
        /// <remarks><see langword="throws"/> an <see cref="InvalidInventoryOperationException"/> if set to a value below 1.</remarks>
        public int Size
        {
            get
            {
                if (m_size < 1)
                    throw new InvalidInventoryOperationException("Inventory cannot be size 0!");
                else
                    return m_size;
            }
        }

        /// <summary>
        /// Is the assigned <see cref="InventorySystem.Inventory"/> full? 
        /// </summary>
        /// <remarks>
        /// <see langword="throws"/> an <see cref="InventoryNotFoundException"/> if assigned <see cref="InventorySystem.Inventory"/> is null.<br/>
        /// Otherwise, <see langword="returns"/> <see cref="Inventory.IsFull"/>.
        /// </remarks>
        public bool AtCapacity
        {
            get
            {
                if (m_inventory == null)
                    throw new InventoryNotFoundException();
                else
                    return Inventory.IsFull;
            }
        }

        protected virtual void Awake()
        {
            m_inventory = new Inventory(m_size);
        }


        /// <summary>
        /// Adds a specified <paramref name="amount"/> of <paramref name="item"/> to the assigned <see cref="InventorySystem.Inventory"/>.
        /// </summary>
        /// <param name="item">The <see cref="ItemData"/> to add.</param>
        /// <param name="amount">The amount of <see cref="ItemData"/> to add.</param>
        /// <returns>If the <paramref name="amount"/> of <paramref name="item"/> was successfully added to the assigned <see cref="InventorySystem.Inventory"/>.</returns>
        public abstract bool AddItem(ItemData item, int amount);

        /// <summary>
        /// Removes a specified <paramref name="amount"/> of <paramref name="item"/> to the assigned <see cref="InventorySystem.Inventory"/>.
        /// </summary>
        /// <param name="item">The <see cref="ItemData"/> to remove.</param>
        /// <param name="amount">The amount of <see cref="ItemData"/> to remove.</param>
        /// <returns>If the <paramref name="amount"/> of <paramref name="item"/> was successfully removed from the assigned <see cref="InventorySystem.Inventory"/>.</returns>
        public abstract bool RemoveItem(ItemData item, int amount);
    }
}