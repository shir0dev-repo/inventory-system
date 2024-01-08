# Shir0dev's Inventory System
<p>Heavily based on Dan Pos' Inventory System tutorial, with a few tweaks and (I dare say) improvements.</p>
<p>The main components of the Inventory System are the Inventory, InventorySlot, and ItemData classes.</p>

<h3>Inventory</h3>
<p>The Inventory class acts as a collection of InventorySlot, adding functionality for adding and removing multiple large quantities of an ItemData at once. </p>

<h3>ItemData</h3>
<p>The ItemData is the core object stored within an InventorySlot. Inheriting from UnityEngine.ScriptableObject, these asset files contain whatever is necessary to have a basic item system up and running.</p>

<h3>InventorySlot</h3>
<p>The runtime wrapper of an ItemData, contained within an Inventory. Inventory slots contain both an item, as well as a stack size, for holding multiple at once. When an inventory is required to add or remove an item, the inventory slots run the logic for checking if there's enough space or items within the slot, and respond accordingly.</p>

<p>Additionally, there are MonoBehaviours such as the ItemHolder, which acts as an instantiation object for the inventory, and allows the use of MonoBehaviour events like Awake and OnCollision, as well as the InventoryDisplay, which acts as both an intermediary for player interaction, as well as a realtime view of the player's inventory.</p>

<p>In the future, I would love to expand this into more than player inventories; storage systems, crafting systems (which is teased slightly in the current code release), as well as support for networking for realtime updated inventory modifications.</p>
