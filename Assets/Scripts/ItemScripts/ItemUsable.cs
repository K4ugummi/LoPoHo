using UnityEngine;

public class ItemUsable : Item {

    [Header("##### Usable Info #####")]
    [SerializeField]
    public float usableRange;

    override public void Accept(VisItemPrimary _vis) { }
    override public void Accept(VisItemReload _vis) { }
}
