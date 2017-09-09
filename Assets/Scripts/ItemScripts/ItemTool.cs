using UnityEngine;

public class ItemTool : Item {

    [Header("##### Usable Info #####")]
    [SerializeField]
    public float toolRange;
    [SerializeField]
    public bool isSelfUsable;

    override public void Accept(VisItemPrimaryDown _vis) { }
    override public void Accept(VisItemPrimaryUp _vis) { }
    override public void Accept(VisItemReload _vis) { }
    public override void Accept(VisItemAmmo _vis) { }
}
