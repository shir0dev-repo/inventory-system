using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Shir0.InventorySystem
{
    public enum SlotClickType { Left, Right, Middle }

    /// <summary>
    /// Extends <see cref="SlotClickType"/> with additional <see langword="static"/> methods.
    /// </summary>
    public static class SlotClickUtils
    {
        /// <summary>
        /// Converts <see cref="PointerEventData"/> to <see cref="SlotClickType"/>.
        /// </summary>
        /// <remarks>If a mouse button not registered in this method is pressed, default to <see cref="SlotClickType.Left"/>.</remarks>
        /// <param name="eventData">Obtained from <see cref="IPointerClickHandler.OnPointerClick(PointerEventData)"/>.</param>
        /// <returns>The <see cref="PointerEventData.InputButton"/> from <paramref name="eventData"/> as a <see cref="SlotClickType"/>.</returns>
        public static SlotClickType GetClickType(this PointerEventData eventData)
            => eventData.button switch
            {
                PointerEventData.InputButton.Left => SlotClickType.Left,
                PointerEventData.InputButton.Right => SlotClickType.Right,
                PointerEventData.InputButton.Middle => SlotClickType.Middle,
                _ => SlotClickType.Left
            };
    }
}