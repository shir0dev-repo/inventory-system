namespace Shir0.InventorySystem.UI
{
    public class HotbarDisplay : InventoryDisplay
    {
        private InventorySlotUI m_lastSelectedHotbarSlot;
        private PlayerHotbarController m_controller;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            m_controller = m_itemHolder.GetComponent<PlayerHotbarController>();
            m_controller.OnHotbarSelected += SelectHotbarSlot;
        }

        protected override void Start()
        {
            base.Start();
            SelectHotbarSlot(1);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_controller.OnHotbarSelected -= SelectHotbarSlot;
        }

        private void SelectHotbarSlot(int hotbarSlotID)
        {
            // adjust ID to fit in zero-indexed array.
            hotbarSlotID -= 1;

            // dehighlight selected slot if possible.
            if (m_lastSelectedHotbarSlot != null)
                ToggleSlotHighlight(m_lastSelectedHotbarSlot, false);

            // assign selected slot.
            m_lastSelectedHotbarSlot = m_uiSlots[hotbarSlotID];

            // highlight reassigned slot.
            ToggleSlotHighlight(m_uiSlots[hotbarSlotID], true);

            UnityEngine.Debug.Log($"Slot {m_uiSlots[hotbarSlotID]} selected!");
        }
    }
}