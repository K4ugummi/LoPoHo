using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class ItemWeapon : Item {
    private const string PLAYER_TAG = "Player";

    private PlayerInteraction playerInteraction;

    [Header("##### Weapon Info #####")]
    [SerializeField]
    private LayerMask hitMask;
    public Transform actionOrigin;
    public float weaponDamage;
    public int weaponMaxClipSize;
    [HideInInspector]
    public int weaponCurClipSize;
    public float weaponRange;
    public float weaponFireRate;
    public float weaponReloadTime;
    private bool isReloading;


    #region Graphic Effects
    [Header("Graphic Effects:")]
    [SerializeField]
    public GameObject muzzleFlash;
    [SerializeField]
    public GameObject hitEffectPrefab;
    #endregion
    #region Sound Effects
    [Header("Sound Effects:")]
    [SerializeField]
    public AudioSource primaryAudio;
    [SerializeField]
    public AudioSource secondaryAudio;
    [SerializeField]
    public AudioSource emptyClipAudio;
    [SerializeField]
    public AudioSource primaryImpactAudio;
    [SerializeField]
    public AudioSource reloadAudio;
    #endregion

    void Start() {
        playerInteraction = GetComponentInParent<PlayerInteraction>();
        if (playerInteraction == null) {
            Debug.LogError("ItemWeapon: playerInteraction == null!");
        }
    }

    #region Visitor
    override public void Accept(VisItemPrimaryDown _vis) {
        if (weaponFireRate <= Mathf.Epsilon ) {
            Primary();
        }
        else {
            if (!IsInvoking("Primary")) {
                InvokeRepeating("Primary", 0f, 1 / weaponFireRate);
            }
        }
    }
    public override void Accept(VisItemReload _vis) {
        if (weaponCurClipSize < weaponMaxClipSize) {
            Reload();
        }
    }
    public override void Accept(VisItemPrimaryUp _vis) {
        CancelInvoke("Primary");
    }
    public override void Accept(VisItemAmmo _vis) {
        _vis.currentClipSize = weaponCurClipSize;
        _vis.maxClipSize = weaponMaxClipSize;
    }

    #endregion
    #region Primary
    void Primary() {
        if (isReloading) {
            return;
        }
        else if (weaponCurClipSize <= 0) {
            playerInteraction.DoPrimaryWeaponEmptyClipEffect();
            CancelInvoke("Primary");
            return;
        }

        weaponCurClipSize--;
        // Call the OnPrimary method on the server
        playerInteraction.OnPrimaryWeapon();
        RaycastHit _hit;
        if (Physics.Raycast(playerInteraction.cam.transform.position, playerInteraction.cam.transform.forward, out _hit, weaponRange, hitMask)) {
            // TODO: Differentiate hit effect and actions!
            // Something has been hit by clicking primary mouse button! 
            // Spawn the on hit effect on the server
            Debug.Log("Shot hit: " + _hit.collider.name);
            playerInteraction.OnPrimaryWeaponHit(_hit.point, _hit.normal);
            switch (_hit.collider.tag) {
                case PLAYER_TAG:
                    playerInteraction.OnPlayerShotWithWeapon(_hit.collider.name, weaponDamage, GetComponentInParent<Player>().transform.name);
                    break;
                default:
                    break;
            }
        }
    }

    public void Reload() {
        if (isReloading) {
            return;
        }
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine() {
        Debug.Log("Reloading...");

        isReloading = true;

        playerInteraction.OnReloadWeapon();

        yield return new WaitForSeconds(weaponReloadTime);

        weaponCurClipSize = weaponMaxClipSize;

        isReloading = false;
    }


    #endregion
}
