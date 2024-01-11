using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shir0.InventorySystem;
using Shir0.InputSystem;
using Shir0.InventorySystem.UI;

public class PlayerHotbarController : MonoBehaviour, IPlayerInputListener
{
    public System.Action<int> OnHotbarSelected;

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

        OnHotbarSelected?.Invoke((int)args.Context.ReadValue<float>());
    }
}
