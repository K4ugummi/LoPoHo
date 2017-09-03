// WIP!!!
// Create classes that derive from PlayerItem (Weapons, Usables, Placeables, ...)
// and use only basic information of what is needed here!
using UnityEngine;

[System.Serializable]
public class PlayerItem : MonoBehaviour {

    [SerializeField]
    public string itemName;
    [SerializeField]
    public int itemDamage;
    [SerializeField]
    public float itemRange;
    [SerializeField]
    public float itemFireRate;
    [SerializeField]
    public int itemMaxAmmo;
    [HideInInspector]
    public int itemAmmo;

    public float itemReloadTime = 1f;

    public GameObject itemGraphics;

    public PlayerItem() {
        itemAmmo = itemMaxAmmo;
    }

    public void SetWeaponAmmoToMax() {
        itemAmmo = itemMaxAmmo;
    }
}
