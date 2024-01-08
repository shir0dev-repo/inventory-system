using Shir0.InventorySystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shir0.InventorySystem.Crafting
{
    public class CraftingStation : MonoBehaviour, IInteractable
    {
        public class CraftRequestArgs : EventArgs
        {
            public Interactor Crafter;
            public ItemHolder ItemHolder;
        }
        [SerializeField] protected RecipeData[] m_recipes;
        public RecipeData[] Recipes => m_recipes;
        public event EventHandler<CraftRequestArgs> OnCraftRequested;

        private void OnEnable()
        {
            OnCraftRequested += Craft;
        }

        private void OnDisable()
        {
            OnCraftRequested -= Craft;
        }

        public bool Interact(Interactor interactor)
        {
            // get the ItemHolder on the Interactor GameObject.
            if (!interactor.TryGetComponent(out ItemHolder itemHolder))
                return false;

            OnCraftRequested?.Invoke(this, new CraftRequestArgs() { Crafter = interactor, ItemHolder = itemHolder });
            Debug.Log(interactor.name + " interacted with " + gameObject.name + "!");
            return true;
        }

        public void Craft(object sender, CraftRequestArgs craftArgs)
        {
            // combined inventory does not have the required items.
            if (!m_recipes[0].Craftable(craftArgs.ItemHolder))
            {
                Debug.Log("cannot craft!");
                return;
            }

            //loop through each recipe component.
            foreach (var recipeComponent in m_recipes[0].RecipeComponents)
            {
                craftArgs.ItemHolder.RemoveItem(recipeComponent);
            }
            craftArgs.ItemHolder.AddItem(m_recipes[0].Output);
            craftArgs.ItemHolder.RequestInventoryRefresh();
        }
    }
}