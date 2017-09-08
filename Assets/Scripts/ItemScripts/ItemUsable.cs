using UnityEngine;

public class ItemUsable : Item {

    [Header("##### Usable Info #####")]
    [SerializeField]
    public float usableRange;

    override public void Accept(VisItemPrimary _vis) {
        // Do usable primary stuff;
    }
}
