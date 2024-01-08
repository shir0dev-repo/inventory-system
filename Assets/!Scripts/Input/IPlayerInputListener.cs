using UnityEngine;

namespace Shir0.InputSystem
{
    public interface IPlayerInputListener
    {
        string ActionName { get; }
        public PlayerInputHandler PreferredSender { get; }
        void PerformAction(object sender, PlayerInputHandler.InputEventArgs args);

        public bool IsActionValid(object sender, PlayerInputHandler.InputEventArgs args)
        {
            if (!sender.Equals(PreferredSender)) return false;
            else if (args.ActionName != ActionName) return false;

            return true;
        }
    }
}