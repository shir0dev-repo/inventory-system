using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shir0.InventorySystem;
using Shir0.InputSystem;
using System;

public class PlayerHotbarController : MonoBehaviour, IPlayerInputListener
{
    public class HotbarSelectArgs : EventArgs
    {
        public int HotbarIndex;
    }
    public static event EventHandler<HotbarSelectArgs> OnHotbarSelected;

    private PlayerInputHandler m_preferredSender;
    public string ActionName => "HotbarSelect";
    public PlayerInputHandler PreferredSender => m_preferredSender;

    private void Awake()
    {
        m_preferredSender = GetComponent<PlayerInputHandler>();
    }

    private void OnEnable()
    {
        PlayerInputHandler.OnInputReceived += PerformAction;
    }

    private void OnDisable()
    {
        PlayerInputHandler.OnInputReceived -= PerformAction;
    }

    public void PerformAction(object sender, PlayerInputHandler.InputEventArgs args)
    {
        if (!sender.Equals(PreferredSender)) return;
        else if (args.ActionName != ActionName) return;

        OnHotbarSelected?.Invoke(this, new HotbarSelectArgs() { HotbarIndex = (int)args.Context.ReadValue<float>() });
        Debug.Log((int)args.Context.ReadValue<float>() + " selected!");
    }
}
