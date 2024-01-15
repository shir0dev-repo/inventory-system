using UnityEngine;

namespace Shir0.InventorySystem.Crafting
{
    [CreateAssetMenu(fileName = "New Recipe", menuName = "Crafting/New Recipe")]
    public class RecipeData : ScriptableObject
    {
        public ItemCountTuple[] RecipeComponents;
        public ItemCountTuple Output;

        public bool Craftable(ItemHolder itemHolder)
        {
            return itemHolder.FindItemRange(RecipeComponents);
        }
    }
}
