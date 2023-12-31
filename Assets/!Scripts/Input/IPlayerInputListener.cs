using UnityEngine;

namespace Shir0.InputSystem
{
    public interface IPlayerInputListener
    {
        string ActionName { get; }
        public PlayerInputHandler PreferredSender { get; }
        void PerformAction(object sender, PlayerInputHandler.InputEventArgs args);
    }
}