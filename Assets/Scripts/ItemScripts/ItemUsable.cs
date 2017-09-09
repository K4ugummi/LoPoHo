using UnityEngine;

public class ItemUsable : Item {

    [Header("##### Usable Info #####")]
    [SerializeField]
    public float usableRange;

    override public void Accept(VisItemPrimaryDown _vis) { }
    override public void Accept(VisItemPrimaryUp _vis) { }
    override public void Accept(VisItemReload _vis) { }
    public override void Accept(VisItemAmmo _vis) { }

}
