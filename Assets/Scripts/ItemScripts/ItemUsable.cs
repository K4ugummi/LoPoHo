using UnityEngine;

public class ItemUsable : Item {

    [Header("##### Usable Info #####")]
    [SerializeField]
    public float usableRange;

    public override void Accept(VisItemPrimaryDown _vis) { }
    public override void Accept(VisItemPrimaryUp _vis) { }
    public override void Accept(VisItemReload _vis) { }
    public override void Accept(VisItemGetAmmo _vis) { }
    public override void Accept(VisItemSetAmmo _vis) { }
    public override void Accept(VisItemSetMaxAmmount _vis) { }

}
