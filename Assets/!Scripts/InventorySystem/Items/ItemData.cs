using UnityEngine;

namespace Shir0.InventorySystem
{
    /// <summary>
    /// Base <see langword="class"/> for creating <see cref="Inventory"/> items, containing an ID, Name, and Description.
    /// </summary>
    /// <remarks>
    /// The <see cref="ItemData"/> <see cref="ScriptableObject"/> is the backbone of the <see cref="InventorySystem"/>.<br/>
    /// In the future, ideally this <see langword="class"/> will become <see langword="abstract"/>, only intended to be<br/>
    /// inherited from, as different <see cref="ItemData"/> will have different functionality.
    /// </remarks>
    [CreateAssetMenu(fileName = "New Item Data", menuName = "Inventory/New Item")]
    public class ItemData : ScriptableObject
    {
        public string Name;
        [TextArea] public string Description;
        public Sprite Sprite;
        public int ID = -1;

        [Space]
        public int MaxStackSize = 99;
        public bool IsSellable;
        [Min(1)] public int SellPrice = 1;
    }
}