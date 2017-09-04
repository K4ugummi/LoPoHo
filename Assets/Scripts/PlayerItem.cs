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
    public float itemReloadTime;
    [SerializeField]
    public int itemMaxAmmo;
    [HideInInspector]
    public int itemAmmo;
    #region Graphic Effects
    [Header("Graphic Effects:")]
    [SerializeField]
    public ParticleSystem muzzleFlash;
    [SerializeField]
    public GameObject hitEffectPrefab;
    #endregion
    #region Sound Effects
    [Header("Sound Effects:")]
    [SerializeField]
    public AudioSource primaryAudio;
    [SerializeField]
    public AudioSource primaryImpactAudio;
    [SerializeField]
    public AudioSource reloadAudio;
    #endregion

    public GameObject itemGraphics;

    public PlayerItem() {
        itemAmmo = itemMaxAmmo;
    }

    public void SetWeaponAmmoToMax() {
        itemAmmo = itemMaxAmmo;
    }

    public ParticleSystem GetMuzzleFlash() {
        return muzzleFlash;
    }
}
