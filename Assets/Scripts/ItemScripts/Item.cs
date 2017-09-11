using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public abstract class Item : MonoBehaviour {
    [Header("##### Basic Item Info #####")]
    [SerializeField]
    public string itemName;
    //public int itemCount = 1;
    public GameObject itemImage;

    public abstract void Accept(VisItemPrimaryDown _vis);
    public abstract void Accept(VisItemReload _vis);
    public abstract void Accept(VisItemPrimaryUp _vis);
    public abstract void Accept(VisItemGetAmmo _vis);
    public abstract void Accept(VisItemSetAmmo _vis);
    public abstract void Accept(VisItemSetMaxAmmount _vis);

}
