namespace Shir0.InventorySystem
{
    public class InvalidInventoryOperationException : System.Exception
    {
        public InvalidInventoryOperationException() { }
        public InvalidInventoryOperationException(string message) : base(message) { }
        public InvalidInventoryOperationException(string message, System.Exception inner) : base(message, inner) { }
    }
}