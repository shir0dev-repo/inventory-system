namespace Shir0.InventorySystem
{

    public class InventoryNotFoundException : System.Exception
    {
        public InventoryNotFoundException() { }
        public InventoryNotFoundException(string message) : base(message) { }
        public InventoryNotFoundException(string message, System.Exception inner) : base(message, inner) { }
    }
}