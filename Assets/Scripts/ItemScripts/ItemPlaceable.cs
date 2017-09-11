using UnityEngine;

public class ItemPlaceable : Item {

    [Header("##### Placeable Info #####")]
    [SerializeField]
    private LayerMask hitMask;
    public GameObject objectToSpawn;
    public float placeableRange;
    PlayerInteraction playerInteraction;
    public int numberOfPlaceables;
    public int maxNumberOfPlaceables;

    void Start() {
        playerInteraction = GetComponentInParent<PlayerInteraction>();
        if (playerInteraction == null) {
            Debug.LogError("ItemWeapon: playerInteraction component not found!");
        }
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
        Debug.DrawRay(playerInteraction.cam.transform.position, playerInteraction.cam.transform.forward * 200f, Color.red, 5f);
        RaycastHit _hit;
        if (Physics.Raycast(playerInteraction.cam.transform.position, playerInteraction.cam.transform.forward, out _hit, placeableRange, hitMask)) {
            playerInteraction.OnPrimaryPlaceable(objectToSpawn, _hit.point);
        }
    }

}
