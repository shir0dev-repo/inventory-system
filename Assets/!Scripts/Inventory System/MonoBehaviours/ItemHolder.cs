using UnityEngine;

namespace Shir0.InventorySystem
{
    [AddComponentMenu(menuName: "Inventory/Item Holder")]
    public class ItemHolder : MonoBehaviour, IPickupHandler
    {
        [SerializeField] private Inventory m_inventory;
        [SerializeField] private int m_size;

        private bool m_atCapacity;

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
        public int Size
        {
            get
            {
                if (m_size <= 0)
                    throw new InvalidInventoryOperationException("Inventory cannot be size 0!");
                else
                    return m_size;
            }
        }
        public bool AtCapacity
        {
            get
            {
                if (m_inventory == null)
                    throw new InventoryNotFoundException();
                else
                    return m_atCapacity;
            }
        }

        private void Awake()
        {
            m_inventory = new Inventory(m_size);
        }

        public void HandleItemPickup(ItemData item, int amount)
        {
            throw new System.NotImplementedException();
        }
        public void DropItem(ref InventorySlot slot, int amount = -1)
        {
            throw new System.NotImplementedException();
        }
    }
}