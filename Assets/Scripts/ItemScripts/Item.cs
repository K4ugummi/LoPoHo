using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public abstract class Item : MonoBehaviour {
    [Header("##### Basic Item Info #####")]
    [SerializeField]
    public string itemName;

    public abstract void Accept(VisItemPrimaryDown _vis);
    public abstract void Accept(VisItemReload _vis);
    public abstract void Accept(VisItemPrimaryUp _vis);
    public abstract void Accept(VisItemAmmo _vis);

}
