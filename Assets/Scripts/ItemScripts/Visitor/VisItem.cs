using UnityEngine;

public abstract class VisItem {

    public abstract void VisItemPlaceable(ItemPlaceable _item);
    public abstract void VisItemTool(ItemTool _item);
    public abstract void VisItemUsable(ItemUsable _item);
    public abstract void VisItemWeapon(ItemWeapon _item);

}
