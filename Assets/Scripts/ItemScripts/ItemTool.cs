using UnityEngine;

public class ItemTool : Item {

    [Header("##### Usable Info #####")]
    [SerializeField]
    public float toolRange;
    [SerializeField]
    public bool isSelfUsable;

    override public void Accept(VisItemPrimary _vis) { }
    override public void Accept(VisItemReload _vis) { }
}
