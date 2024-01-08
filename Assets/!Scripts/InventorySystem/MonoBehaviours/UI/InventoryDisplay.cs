using System.Collections.Generic;
using UnityEngine;

namespace Shir0.InventorySystem
{
    /// <summary>
    /// A collection of UI slots linked to an inventory.
    /// </summary>
    public class InventoryDisplay : MonoBehaviour
    {
        [SerializeField] private MouseInventory m_mouseInventory;
        [SerializeField] private ItemHolder m_itemHolder;
        [SerializeField] private InventorySlotUI[] m_uiSlots;

        private readonly Dictionary<InventorySlotUI, InventorySlot> m_slotDictionary = new();

        private void OnEnable()
        {
            InventorySlotUI.OnUISlotClicked += HandleUISlotInteraction;
            if (m_itemHolder != null)
            {
                m_itemHolder.OnInventoryDisplayRequested -= RefreshInventory;
                m_itemHolder.OnInventoryDisplayRequested += RefreshInventory;
            }
        }

        private void Start()
        {
            if (m_mouseInventory == null)
                Debug.LogError("Mouse inventory slot not found!");

            if (m_itemHolder == null)
                throw new InventoryNotFoundException("Item holder could not be found!");

            InitializeSlots(m_itemHolder.Inventory);
        }

        private void OnDisable()
        {
            InventorySlotUI.OnUISlotClicked -= HandleUISlotInteraction;
            m_itemHolder.OnInventoryDisplayRequested -= RefreshInventory;
        }

        /// <summary>
        /// Assigns each element in the UI slot array to its corresponding slot.
        /// </summary>
        /// <param name="inventory">The inventory to display.</param>
        /// <exception cref="SlotMismatchException"/>
        void InitializeSlots(Inventory inventory)
        {
            if (m_uiSlots.Length != inventory.Size)
                throw new SlotMismatchException("Display must be the same size as underlying Inventory!");

            for (int i = 0; i < inventory.Size; i++)
            {
                m_slotDictionary.Add(m_uiSlots[i], inventory[i]);
                m_uiSlots[i].InitSlot(inventory[i]);
            }
        }

        /// <summary>
        /// Updates every UI slot displaying the item holder's inventory.
        /// </summary>
        private void RefreshInventory()
        {
            foreach (InventorySlotUI uiSlot in m_uiSlots)
                uiSlot.UpdateSlot();
        }

        /// <summary>
        /// Handles inventory functionality for iteracting with a UI slot.
        /// </summary>
        /// <param name="sender">The slot sending the interaction.</param>
        /// <param name="interactionArgs">The arguments for the interaction.</param>
        private void HandleUISlotInteraction(object sender, InventorySlotUI.UISlotInteractionArgs interactionArgs)
        {
            if (!m_slotDictionary.ContainsKey(sender as InventorySlotUI))
                return;

            InventorySlotUI interactedSlot = sender as InventorySlotUI;

            switch (interactionArgs.InteractionType)
            {
                case SlotInteraction.HoverEnter:
                    break;
                case SlotInteraction.HoverExit:
                    break;
                case SlotInteraction.ClickLeft:
                    HandleUIClickLeft(interactedSlot);
                    break;
                case SlotInteraction.ClickMiddle:
                    HandleUIClickMiddle(interactedSlot);
                    break;
                case SlotInteraction.ClickRight:
                    HandleUIClickRight(interactedSlot);
                    break;
            }

            interactedSlot.UpdateSlot();
            m_mouseInventory.UpdateMouseUI();
        }

        /// <summary>
        /// Handles <see cref="SlotInteraction.ClickLeft"/> functionality.
        /// </summary>
        /// <param name="clickedSlot">The slot that was clicked.</param>
        private void HandleUIClickLeft(InventorySlotUI clickedSlot)
        {
            // both mouse and clicked slot are empty: do nothing.
            if (!clickedSlot.AssignedSlot.HasItem && !m_mouseInventory.Slot.HasItem)
                return;

            // slot has item and mouse is empty: pick up slot item stack into mouse slot.
            else if (clickedSlot.AssignedSlot.HasItem && !m_mouseInventory.Slot.HasItem)
            {
                m_mouseInventory.AssignMouseSlot(clickedSlot.AssignedSlot);
                clickedSlot.AssignedSlot.ClearSlot();
                return;
            }

            // mouse has item and slot is empty: place mouse into slot.
            else if (!clickedSlot.AssignedSlot.HasItem && m_mouseInventory.Slot.HasItem)
            {
                clickedSlot.AssignedSlot.AssignSlot(m_mouseInventory.Slot.CurrentItem, m_mouseInventory.Slot.CurrentStackSize);
                m_mouseInventory.Slot.ClearSlot();
                return;
            }
            // both mouse and slot have items: decide course of action.
            else
            {
                // different items: swap slots.
                if (clickedSlot.AssignedSlot.CurrentItem != m_mouseInventory.Slot.CurrentItem)
                {
                    InventorySlot clonedSlot = new InventorySlot(clickedSlot.AssignedSlot);
                    clickedSlot.AssignedSlot.AssignSlot(m_mouseInventory.Slot);
                    m_mouseInventory.AssignMouseSlot(clonedSlot);
                    return;
                }

                // same items: check combined stack sizes.
                clickedSlot.AssignedSlot.AddToStack(m_mouseInventory.Slot.CurrentItem, m_mouseInventory.Slot.CurrentStackSize, out int remainder);

                // could not fully add items: assign remainder to mouse.
                if (remainder > 0)
                    m_mouseInventory.AssignMouseSlot(new ItemCountTuple(m_mouseInventory.Slot.CurrentItem, remainder));
                else // items fully added to slot: clear mouse.
                    m_mouseInventory.Slot.ClearSlot();
            }
        }

        /// <summary>
        /// Handles <see cref="SlotInteraction.ClickMiddle"/> functionality.
        /// </summary>
        /// <param name="clickedSlot">The slot that was clicked.</param>
        private void HandleUIClickMiddle(InventorySlotUI clickedSlot)
        {
            // slot empty, mouse empty: do nothing.
            if (!clickedSlot.AssignedSlot.HasItem && !m_mouseInventory.Slot.HasItem)
                return;

            // slot has item, mouse empty: place half from slot into mouse.
            if (clickedSlot.AssignedSlot.HasItem && !m_mouseInventory.Slot.HasItem)
                m_mouseInventory.AssignMouseSlot(clickedSlot.AssignedSlot.SplitStack());

            // slot empty, mouse has item: place half from mouse into slot.
            else if (!clickedSlot.AssignedSlot.HasItem && m_mouseInventory.Slot.HasItem)
                clickedSlot.AssignedSlot.AssignSlot(m_mouseInventory.Slot.SplitStack());

            // slot has item, mouse has item: decide course of action.
            else
            {
                // slot item != mouse item: swap slots.
                if (clickedSlot.AssignedSlot.CurrentItem != m_mouseInventory.Slot.CurrentItem)
                {
                    SwapSlots(clickedSlot);
                    return;
                }

                // slot item == mouse item: compare stack sizes.
                InventorySlot splitSlot = clickedSlot.AssignedSlot.SplitStack();

                // mouse can fully fit split stack: add to mouse.
                if (m_mouseInventory.Slot.AddToStack(splitSlot.CurrentItem, splitSlot.CurrentStackSize, out _))
                    m_mouseInventory.UpdateMouseUI();

                // mouse cannot fully fit split stack: fill mouse.
                else
                    m_mouseInventory.UpdateMouseUI();
            }
        }

        /// <summary>
        /// Handles <see cref="SlotInteraction.ClickRight"/> functionality.
        /// </summary>
        /// <param name="clickedSlot">The slot that was clicked.</param>
        private void HandleUIClickRight(InventorySlotUI clickedSlot)
        {
            // slot empty, mouse empty: do nothing.
            if (!clickedSlot.AssignedSlot.HasItem && !m_mouseInventory.Slot.HasItem) return;

            // slot has item, mouse empty: place one from slot into mouse.
            if (clickedSlot.AssignedSlot.HasItem && !m_mouseInventory.Slot.HasItem)
            {
                m_mouseInventory.Slot.AssignSlot(clickedSlot.AssignedSlot.CurrentItem, 1);
                clickedSlot.AssignedSlot.RemoveFromStack(clickedSlot.AssignedSlot.CurrentItem, 1, out _);
            }

            // slot empty, mouse has item: place one from mouse into slot.
            else if (!clickedSlot.AssignedSlot.HasItem && m_mouseInventory.Slot.HasItem)
            {
                clickedSlot.AssignedSlot.AssignSlot(m_mouseInventory.Slot.CurrentItem, 1);
                m_mouseInventory.Slot.RemoveFromStack(m_mouseInventory.Slot.CurrentItem, 1, out _);
            }

            // slot has item, mouse has item: decide course of action.
            else
            {
                // differing items: swap slots.
                if (clickedSlot.AssignedSlot.CurrentItem != m_mouseInventory.Slot.CurrentItem)
                    SwapSlots(clickedSlot);

                // same items: take one from slot, add to mouse.
                else if (m_mouseInventory.Slot.AddToStack(clickedSlot.AssignedSlot.CurrentItem, 1, out int _))
                    clickedSlot.AssignedSlot.RemoveFromStack(clickedSlot.AssignedSlot.CurrentItem, 1, out _);
            }
        }

        private void SwapSlots(InventorySlotUI clickedSlot)
        {
            InventorySlot clonedSlot = new InventorySlot(clickedSlot.AssignedSlot);
            clickedSlot.AssignedSlot.AssignSlot(m_mouseInventory.Slot);
            m_mouseInventory.AssignMouseSlot(clonedSlot);
        }
    }
}