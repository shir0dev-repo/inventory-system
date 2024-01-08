using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shir0.InputSystem;

public class Interactor : MonoBehaviour, IPlayerInputListener
{
    [SerializeField] private LayerMask m_interactionLayer;

    private PlayerInputHandler m_preferredSender;
    public string ActionName => "Interact";
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

        RaycastHit2D[] results = Physics2D.LinecastAll(transform.position, transform.position + transform.up, m_interactionLayer);
        if (results.Length < 1)
            return;

        foreach (RaycastHit2D rcHit in results)
        {
            if (rcHit.collider.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact(this);
                return;
            }
        }
    }
}
