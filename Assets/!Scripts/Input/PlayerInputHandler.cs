using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shir0.InputSystem
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public class InputEventArgs : EventArgs
        {
            public InputAction.CallbackContext Context;
            public string ActionName;
        }

        public static event EventHandler<InputEventArgs> OnInputReceived;

        private PlayerInputActionsAsset m_actionsAsset;

        private void Awake()
        {
            m_actionsAsset = new();
            foreach (InputAction action in m_actionsAsset)
            {
                action.performed += HandleAction;
            }
        }

        private void OnEnable() => m_actionsAsset.Enable();
        private void OnDisable()
        {
            foreach (InputAction action in m_actionsAsset)
            {
                action.started -= HandleAction;
            }
            m_actionsAsset.Disable();
        }

        void HandleAction(InputAction.CallbackContext context)
            => OnInputReceived?.Invoke(this, new InputEventArgs { Context = context, ActionName = context.action.name });
        
    }
}