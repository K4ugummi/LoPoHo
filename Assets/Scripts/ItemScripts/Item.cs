using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public abstract class Item : NetworkBehaviour {
    [Header("##### Basic Item Info #####")]
    [SerializeField]
    public string itemName;
    [SerializeField]
    public GameObject itemGraphics;

    public abstract void Accept(VisItemPrimary _vis);

}
