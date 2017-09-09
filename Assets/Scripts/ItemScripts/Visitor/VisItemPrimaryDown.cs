using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisItemPrimary : VisItem {

    public override void VisItemPlaceable(ItemPlaceable _item) {
        TYPE = ItemPrimaryType.Placeable;
        _item.Accept(this);
    }
    public override void VisItemTool(ItemTool _item) {
        TYPE = ItemPrimaryType.Tool;
        _item.Accept(this);
    }
    public override void VisItemUsable(ItemUsable _item) {
        TYPE = ItemPrimaryType.Usable;
        _item.Accept(this);
    }
    override public void VisItemWeapon(ItemWeapon _item) {
        TYPE = ItemPrimaryType.Weapon;
        _item.Accept(this);
    }
}
