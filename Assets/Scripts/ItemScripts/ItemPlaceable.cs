using UnityEngine;

public class ItemPlaceable : Item {

    [Header("##### Placeable Info #####")]
    public float placeableRange;

    override public void Accept(VisItemPrimaryDown _vis) { }
    override public void Accept(VisItemPrimaryUp _vis) { }
    override public void Accept(VisItemReload _vis) { }
    public override void Accept(VisItemAmmo _vis) { }

}
