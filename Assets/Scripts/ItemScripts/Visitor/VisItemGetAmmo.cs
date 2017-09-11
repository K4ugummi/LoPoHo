using UnityEngine;

public class VisItemGetAmmo : VisItem {

    public int currentClipSize;
    public int maxClipSize;

    public override void VisItemPlaceable(ItemPlaceable _item) {
        _item.Accept(this);
    }
    public override void VisItemTool(ItemTool _item) {
        _item.Accept(this);
    }
    public override void VisItemUsable(ItemUsable _item) {
        _item.Accept(this);
    }
    override public void VisItemWeapon(ItemWeapon _item) {
        _item.Accept(this);
    }
}
