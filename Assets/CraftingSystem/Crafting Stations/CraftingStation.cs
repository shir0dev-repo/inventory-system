using System;
using UnityEngine;

namespace Shir0.InventorySystem.Crafting
{
    public class CraftingStation : MonoBehaviour
    {
        [SerializeField] protected RecipeData[] m_recipes;
        
        public RecipeData[] Recipes => m_recipes;

        public void Craft(ItemHolder holder)
        {
            // combined inventory does not have the required items.
            if (!m_recipes[0].Craftable(holder))
            {
                Debug.Log("cannot craft!");
                return;
            }

            //loop through each recipe component.
            foreach (var recipeComponent in m_recipes[0].RecipeComponents)
            {
                holder.RemoveItem(recipeComponent);
            }

            holder.AddItem(m_recipes[0].Output);
            holder.RequestInventoryRefresh();
        }
    }
}