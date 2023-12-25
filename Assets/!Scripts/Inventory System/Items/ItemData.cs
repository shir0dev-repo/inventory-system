using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public string Name;
    [TextArea] public string Description;
    public int ID = -1;

    [Space]
    public int MaxStackSize = 99;
    public bool IsSellable;
    [Min(1)] public int SellPrice = 1;
}
