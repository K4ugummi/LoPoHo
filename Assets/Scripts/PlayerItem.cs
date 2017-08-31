// WIP!!!
// Create classes that derive from PlayerItem (Weapons, Usables, Placeables, ...)
// and use only basic information of what is needed here!
using UnityEngine;

[System.Serializable]
public class PlayerItem {

    public string name = "Glock";

    public int damage = 10;
    public float range = 100f;
    public float fireRate = 10f;

    public int maxAmmo = 20;
    [HideInInspector]
    public int ammo;

    public float reloadTime = 1f;

    public GameObject graphics;

    public PlayerItem() {
        ammo = maxAmmo;
    }
}
