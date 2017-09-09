using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public abstract class Item : MonoBehaviour {
    [Header("##### Basic Item Info #####")]
    [SerializeField]
    public string itemName;

    public abstract void Accept(VisItemPrimary _vis);
    public abstract void Accept(VisItemReload _vis);

}
