using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemWeapon : Item {
    private const string PLAYER_TAG = "Player";

    [Header("##### Weapon Info #####")]
    [SerializeField]
    private LayerMask hitMask;
    public Transform actionOrigin;
    public float weaponDamage;
    public int weaponMaxClipSize;
    public int weaponCurClipSize;
    public float weaponRange;
    public float weaponFireRate;
    public float weaponReloadTime;
    private bool isReloading;


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
    public AudioSource secondaryAudio;
    [SerializeField]
    public AudioSource emptyClipAudio;
    [SerializeField]
    public AudioSource primaryImpactAudio;
    [SerializeField]
    public AudioSource reloadAudio;
    #endregion

    public ItemWeapon() {
        weaponCurClipSize = weaponMaxClipSize;
    }

    #region Visitor
    override public void Accept(VisItemPrimary _vis) {
        Debug.Log("VisItemPrimary");
        Primary();
    }

    #endregion
    #region Primary
    [Client]
    void Primary() {
        if (isReloading || weaponCurClipSize <= 0) {
            return;
        }

        weaponCurClipSize--;

        // Call the OnPrimary method on the server
        CmdOnPrimary();

        RaycastHit _hit;
        if (Physics.Raycast(actionOrigin.transform.position, actionOrigin.transform.forward, out _hit, weaponRange, hitMask)) {
            // TODO: Differentiate hit effect and actions!
            // Something has been hit by clicking primary mouse button! 
            // Spawn the on hit effect on the server
            Debug.Log("Shot hit: " + _hit.collider.name);
            CmdOnPrimaryHit(_hit.point, _hit.normal);
            switch (_hit.collider.tag) {
                case PLAYER_TAG:
                    CmdOnPlayerShot(_hit.collider.name, weaponDamage, transform.name);
                    break;
                default:
                    break;
            }
        }
    }

    [Command]
    void CmdOnPrimary() {
        RpcDoPrimaryEffect();
    }

    // Do on primary effect on all clients
    [ClientRpc]
    void RpcDoPrimaryEffect() {
        //PlayerItem _item = itemManager.GetCurrentItem();
        //if (_item == null) {
        //    return;
        //}
        // TODO: FIX THIS SHIT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        muzzleFlash.Play();
        AudioSource.PlayClipAtPoint(primaryAudio.clip, transform.position, 0.5f);
    }

    // Is called on the server, when the primary mouse button action has hit something
    [Command]
    void CmdOnPrimaryHit(Vector3 _hitPosition, Vector3 _normalOfSurface) {
        RpcDoPrimaryHitEffect(_hitPosition, _normalOfSurface);
    }

    // Do on primary hit effect on all clients
    [ClientRpc]
    void RpcDoPrimaryHitEffect(Vector3 _hitPosition, Vector3 _normalOfSurface) {
        //if (_item == null) {
        //    return;
        //}
        // TODO: Instantiation objects takes a lot of processing power
        // Look into "object pooling"
        GameObject _hitEffect = Instantiate(hitEffectPrefab, _hitPosition, Quaternion.LookRotation(_normalOfSurface));
        AudioSource.PlayClipAtPoint(primaryImpactAudio.clip, _hitPosition, 0.5f);
        Destroy(_hitEffect, 1f);
    }

    [Command]   // only called on the server!
    void CmdOnPlayerShot(string _playerID, float _damage, string _sourceID) {
        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage, _sourceID);
    }
    #endregion

    public void Reload() {
        if (isReloading) {
            return;
        }
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine() {
        Debug.Log("Reloading...");

        isReloading = true;

        CmdOnReload();

        yield return new WaitForSeconds(weaponReloadTime);

        weaponCurClipSize = weaponMaxClipSize;

        isReloading = false;
    }

    [Command]
    void CmdOnReload() {
        RpcOnReload();
    }

    [ClientRpc]
    void RpcOnReload() {
        //Animator _animator = currentItem.GetComponent<Animator>();
        //if (_animator != null) {
        //    _animator.SetTrigger("Reload");
        //}
    }
}
