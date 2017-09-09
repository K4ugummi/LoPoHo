using UnityEngine;

public class ItemPlaceable : Item {

    [Header("##### Placeable Info #####")]
    public float placeableRange;

    override public void Accept(VisItemPrimary _vis) { }
    override public void Accept(VisItemReload _vis) { }
}
