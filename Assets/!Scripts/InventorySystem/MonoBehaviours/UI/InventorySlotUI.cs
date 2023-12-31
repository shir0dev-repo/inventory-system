using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Shir0.InventorySystem
{
    /// <summary>
    /// User Interface for displaying it's corresponding <see cref="InventorySlot"/>.
    /// </summary>
    /// <remarks>
    /// The core functionality of <see cref="InventorySlotUI"/> is the <see cref="OnUISlotClicked"/> event. When invoked,<br/>
    /// the event will fire, passing in a <see cref="UISlotClickArgs"/>, containing it's assigned <see cref="InventorySlot"/><br/>
    /// and the type of mouseclick, denoted by <see cref="SlotClickType"/>, registered by Unity's EventSystem.<br/><br/>
    /// Primarily, this event is used to pass the user interaction with the <see cref="InventorySlotUI"/> to the internal<br/>
    /// <see cref="InventorySlot"/>. When invoked, listener's should update this <see cref="InventorySlotUI"/> using <br/>
    /// <see cref="UpdateSlot"/>, to reflect the internal changes. The changes affected are displayed on the <br/>
    /// <see cref="TextMeshProUGUI"/> and <see cref="Image"/> components attached to the <see cref="GameObject"/>.<br/><br/>
    /// Additionally, instead of using Unity's <see cref="Button"/> for registering clicks, <see cref="InventorySlotUI"/> implements the<br/>
    /// <see cref="IPointerClickHandler"/> <see langword="interface"/>, allowing the differentiation of left, middle, and right clicks using<br/>
    /// <see cref="PointerEventData.InputButton"/>. This allows for easier implementation to handle multiple types of clicks.
    /// </remarks>
    public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>
        /// An <see langword="event"/> argument for <see cref="OnUISlotClicked"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="UISlotClickArgs"/> acts as an intermediary for invoking <see cref="OnUISlotClicked"/>. By invoking the <see langword="event"/><br/>
        /// with a sender <see langword="object"/> and a <see cref="SlotClickType"/>, subscribers can check<br/>
        /// within their <see cref="Inventory"/> to see if they own the  that sent the <see langword="event"/>.<br/><br/>
        /// Currently, as the <see langword="event"/> is marked <see langword="static"/>, any behaviour can subscribe and intercept <see cref="OnUISlotClicked"/>.<br/>
        /// Down the line this may cause issues, as it opens the potential of external listeners to read which <see cref="InventorySlot"/><br/>
        /// is being referenced and manipulate an <see cref="Inventory"/> it should not have access to.<br/><br/>
        /// A potential fix might be to move the <see langword="event"/> to the <see cref="InventoryDisplay"/> and remove its <see langword="static"/> property, and<br/>
        /// force the <see cref="InventorySlotUI"/>[] held by the display to subscribe, rendering the event effectively privatized within the scope.
        /// </remarks>
        public class UISlotClickArgs : EventArgs
        {
            /// <summary>
            /// The <see cref="SlotClickType"/> provided by <see cref="IPointerClickHandler.OnPointerClick(PointerEventData)"/>.
            /// </summary>
            public SlotClickType ClickType;
        }

        /// <summary>
        /// The <see langword="event"/> called when a <see cref="InventorySlotUI"/> is clicked.
        /// </summary>
        public static event EventHandler<UISlotClickArgs> OnUISlotClicked;

        /// <summary>
        /// The <see cref="TextMeshProUGUI"/> component displaying the stack size of the currently assigned <see cref="InventorySlot"/>.
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_stackSize;

        /// <summary>
        /// The <see cref="Image"/> component displaying the <see cref="Sprite"/> of the currently assigned <see cref="InventorySlot"/>.
        /// </summary>
        [SerializeField] private Image m_icon;

        /// <summary>
        /// The internal <see cref="InventorySlot"/> assigned to this <see cref="InventorySlotUI"/> display.
        /// </summary>
        private InventorySlot m_assignedSlot;

        /// <summary>
        /// Initializes the <see cref="InventorySlotUI"/> by setting its <paramref name="assignedSlot"/> and updating its display.
        /// </summary>
        /// <remarks>
        /// By forcing the caller to supply an <see cref="InventorySlot"/>, this allows the <see cref="InventorySlotUI"/><br/>
        /// to always be linked to an <see cref="InventorySlot"/>.<br/><br/>
        /// Additionally, by calling <see cref="UpdateSlot"/>, the slot will either be cleared<br/>
        /// or updated with the data contained within the <paramref name="assignedSlot"/>. This effectively future-<br/>
        /// proofs <see cref="InventorySlotUI"/>, by allowing already an existing <see cref="Inventory"/> (possibly through<br/>
        /// saved <see cref="JsonUtility"/> data) to instantly be reflected through the <see cref="InventorySlotUI"/>.
        /// </remarks>
        /// <param name="assignedSlot">The supplied <see cref="InventorySlot"/> to link to this <see cref="InventorySlotUI"/>.</param>
        public void Init(InventorySlot assignedSlot)
        {
            m_assignedSlot = assignedSlot;
            UpdateSlot();
        }

        /// <summary>
        /// Reflects any past changes to the assigned <see cref="InventorySlot"/>.
        /// </summary>
        /// <exception cref="SlotMismatchException"></exception>
        public void UpdateSlot()
        {
            if (m_assignedSlot == null)
                throw new SlotMismatchException("Assigned slot is null!");

            if (m_assignedSlot.CurrentItem == null)
            {
                m_icon.sprite = null;
                m_icon.color = Color.clear;
            }
            else
            {
                m_icon.sprite = m_assignedSlot.CurrentItem.Sprite;
                m_icon.color = Color.white;
            }

            if (m_assignedSlot.CurrentStackSize > 1)
                m_stackSize.text = m_assignedSlot.CurrentStackSize.ToString();
            else
                m_stackSize.text = string.Empty;
        }

        /// <summary>
        /// Method inherited from <see cref="IPointerClickHandler"/> which invokes the <see cref="OnUISlotClicked"/> <see langword="event"/>.
        /// </summary>
        /// <param name="eventData"><see cref="PointerEventData"/> supplied by Unity's <see cref="EventSystem"/>.</param>
        public void OnPointerClick(PointerEventData eventData)
            => OnUISlotClicked?.Invoke(this, new UISlotClickArgs { ClickType = eventData.GetClickType() });
    }
}