using UnityEngine;

namespace Shir0.InventorySystem
{
    /// <summary>
    /// Collidable object holding an amount of <see cref="ItemData"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="ItemPickup"/> is intended to be instantiated when a condition is met (i.e. enemy item drop, dropped item, etc.).<br/>
    /// Upon colliding with a <see cref="GameObject"/>, the <see cref="ItemPickup"/> will attempt to find an <see cref="ItemHolder"/> component.<br/>
    /// If  successful, add the <see cref="ItemData"/> attached to the <see cref="ItemPickup"/>, and destroy this <see cref="GameObject"/>.
    /// <br/><br/>
    /// As collision detection begins the frame after the <see cref="GameObject"/> is instantiated, a timer<br/>
    /// must restrict the <see cref="ItemPickup"/>, to be able to drop the item from the player's <see cref="Inventory"/>.
    /// </remarks>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CircleCollider2D))]
    [AddComponentMenu(menuName: "Inventory/Item Pickup")]
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] private ItemData m_item;
        [SerializeField, Min(1)] private int m_itemCount;
        [SerializeField] private float m_pickupCooldown = 1.5f;

        bool m_canPickup = false;

        /// <summary>
        /// Assigns an amount of <see cref="ItemData"/> to the pickup.
        /// </summary>
        /// <param name="item">The <see cref="ItemData"/> to assign.</param>
        /// <param name="itemCount">The amount of <see cref="ItemData"/> to assign.</param>
        public void AssignItem(ItemData item, int itemCount)
        {
            m_item = item;
            m_itemCount = itemCount;
        }

        private void Update()
        {
            if (!m_canPickup)
            {
                m_pickupCooldown -= Time.deltaTime;
                if (m_pickupCooldown <= 0)
                    m_canPickup = true;
            }
        }
    }
}