// WIP!!!
// Create classes that derive from PlayerItem (Weapons, Usables, Placeables, ...)
// and use only basic information of what is needed here!
using UnityEngine;

[System.Serializable]
public class PlayerItem : MonoBehaviour {

    public string itemName = "Glock";

    public int itemDamage = 10;
    public float itemRange = 100f;
    public float itemFireRate = 10f;

    public int itemMaxAmmo = 20;
    [HideInInspector]
    public int itemAmmo;

    public float itemReloadTime = 1f;

    public GameObject itemGraphics;

    public PlayerItem() {
        itemAmmo = itemMaxAmmo;
    }
}
