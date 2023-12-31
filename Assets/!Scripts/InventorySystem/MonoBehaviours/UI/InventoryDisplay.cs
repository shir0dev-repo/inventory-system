using System.Collections.Generic;
using UnityEngine;

namespace Shir0.InventorySystem
{
    /// <summary>
    /// A collection of <see cref="InventorySlotUI"/> linked to an <see cref="ItemHolder.Inventory"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="InventoryDisplay"/> acts as an intermediary between the <see cref="ItemHolder"/> behaviour<br/>
    /// and User Interface by assigning a KeyValuePair to the <see cref="InventorySlotUI"/>[] to an <br/>
    /// <see cref="ItemHolder.Inventory"/>. 
    /// </remarks>
    public class InventoryDisplay : MonoBehaviour
    {
        [SerializeField] private ItemHolder m_itemHolder;
        [SerializeField] private InventorySlotUI[] m_uiSlots;
        [SerializeField] private ItemData testItem;

        private Inventory m_inventory;
        private Dictionary<InventorySlotUI, InventorySlot> m_slotDictionary = new();

        private void Start()
        {
            m_inventory = m_itemHolder.Inventory;

            InitializeSlots(m_inventory);

            InventorySlotUI.OnUISlotClicked += HandleUISlotClick;
            m_itemHolder.OnInventoryDisplayRequested += RefreshInventory;
        }

        private void RefreshInventory(object sender, System.EventArgs e)
        {
            if ((sender as ItemHolder) != m_itemHolder)
                return;

            foreach (InventorySlotUI uiSlot in m_uiSlots)
                uiSlot.UpdateSlot();
        }

        /// <summary>
        /// Assigns each element in the <see cref="InventorySlotUI"/>[] to its corresponding <see cref="InventorySlot"/>.
        /// </summary>
        /// <param name="inventory">The inventory to display.</param>
        /// <exception cref="SlotMismatchException"></exception>
        /// 
        void InitializeSlots(Inventory inventory)
        {
            if (m_uiSlots.Length != inventory.Size)
                throw new SlotMismatchException("Display must be the same size as underlying Inventory!");

            for (int i = 0; i < inventory.Size; i++)
            {
                m_slotDictionary.Add(m_uiSlots[i], inventory[i]);
                m_uiSlots[i].Init(inventory[i]);
            }
        }

        /// <summary>
        /// Handles <see cref="Inventory"/> functionality from the <see cref="InventorySlotUI"/> <paramref name="sender"/>.
        /// </summary>
        /// <param name="sender">The <see cref="InventorySlotUI"/> sending the request.</param>
        /// <param name="clickArgs">The arguments sent by <paramref name="sender"/>. Currently, this only contains the <see cref="SlotClickType"/>.</param>
        private void HandleUISlotClick(object sender, InventorySlotUI.UISlotClickArgs clickArgs)
        {
            if (!m_slotDictionary.ContainsKey(sender as InventorySlotUI))
                return;

            InventorySlotUI clickedSlot = sender as InventorySlotUI;
            if (clickArgs.ClickType == SlotClickType.Left)
            {
                m_slotDictionary[clickedSlot].AddToStack(testItem, 2, out _);
            }
            else if (clickArgs.ClickType == SlotClickType.Right)
            {
                m_slotDictionary[clickedSlot].RemoveFromStack(20, out _);
            }
            clickedSlot.UpdateSlot();
        }
    }
}