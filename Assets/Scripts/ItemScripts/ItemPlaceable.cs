using UnityEngine;

public class ItemPlaceable : Item {

    const string GROUND_TAG = "Ground";

    [Header("##### Placeable Info #####")]
    [SerializeField]
    private LayerMask hitMask;
    public GameObject objectToSpawn;
    Vector3 dimensions;
    public float placeableRange;
    PlayerInteraction playerInteraction;
    public int numberOfPlaceables;
    public int maxNumberOfPlaceables;

    void Start() {
        playerInteraction = GetComponentInParent<PlayerInteraction>();
        if (playerInteraction == null) {
            Debug.LogError("ItemWeapon: playerInteraction component not found!");
        }
        dimensions = objectToSpawn.transform.lossyScale;
    }

public override void Accept(VisItemPrimaryDown _vis) {
        Primary();
    }
    public override void Accept(VisItemPrimaryUp _vis) { }
    public override void Accept(VisItemReload _vis) { }
    public override void Accept(VisItemGetAmmo _vis) { }
    public override void Accept(VisItemSetAmmo _vis) { }
    public override void Accept(VisItemSetMaxAmmount _vis) { }

    void Primary() {
        Debug.Log("Placeable Primary!");
        RaycastHit _hit;
        if (Physics.Raycast(playerInteraction.cam.transform.position, playerInteraction.cam.transform.forward, out _hit, placeableRange, hitMask)) {
            Vector3 gridAlligner = _hit.point;
            if (_hit.collider.tag != GROUND_TAG) {
                gridAlligner += _hit.normal * 0.5f;
            }
            gridAlligner.x = Mathf.Round(gridAlligner.x);
            gridAlligner.y = Mathf.Round(gridAlligner.y);
            gridAlligner.z = Mathf.Round(gridAlligner.z);
            playerInteraction.OnPrimaryPlaceable(objectToSpawn, gridAlligner);
        }
    }

}
