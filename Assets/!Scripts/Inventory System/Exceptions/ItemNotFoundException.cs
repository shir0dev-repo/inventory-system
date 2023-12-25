using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemNotFoundException : System.Exception
{
    public ItemNotFoundException() { }
    public ItemNotFoundException(string message) : base(message) { }
    public ItemNotFoundException(string message, System.Exception inner) : base(message, inner) { }
}
