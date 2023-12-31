using UnityEngine;
using Shir0.InputSystem;
using UnityEngine.InputSystem;

namespace Shir0.InventorySystem
{
    public class PlayerItemHolder : ItemHolder, IPlayerInputListener
    {
        [SerializeField] private Inventory m_hotbarInventory;
        [SerializeField] private int m_hotbarSize;
        public ItemData itemData;

        private PlayerInputHandler m_inputHandler;

        public string ActionName
        {
            get { return "HotbarSelect"; }
        }

        public PlayerInputHandler PreferredSender
        {
            get
            {
                if (m_inputHandler == null)
                    m_inputHandler = GetComponent<PlayerInputHandler>();

                return m_inputHandler;
            }
        }

        public Inventory HotbarInventory
        {
            get { return m_hotbarInventory; }
        }

        public int HotbarSize
        {
            get { return m_hotbarSize; }
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                m_inventory.AddItem(itemData, 1);
        }

        public override bool AddItem(ItemData item, int amount)
        {
            throw new System.NotImplementedException();
        }

        public override bool RemoveItem(ItemData item, int amount)
        {
            throw new System.NotImplementedException();
        }

        public void PerformAction(object sender, PlayerInputHandler.InputEventArgs args)
        {
            if ((sender as PlayerInputHandler) != PreferredSender)
                return;

            int hotbarSlotID = args.Context.ReadValue<int>();
            HandleHotbarSelection(hotbarSlotID);
        }

        public void HandleHotbarSelection(int hotbarSlotID)
        {
            Debug.Log($"Hotbar slot {hotbarSlotID} selected!");
        }
    }
}