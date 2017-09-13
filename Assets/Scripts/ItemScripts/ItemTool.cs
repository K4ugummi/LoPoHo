using UnityEngine;

public class ItemTool : Item {
    
    private PlayerInteraction playerInteraction;

    [Header("##### Tool Info #####")]
    [SerializeField]
    private LayerMask hitMask;
    [SerializeField]
    public float toolRange;


    void Start() {
        playerInteraction = GetComponentInParent<PlayerInteraction>();
        if (playerInteraction == null) {
            Debug.LogError("ItemTool: playerInteraction component not found!");
        }
    }

    #region Visitor
    public override void Accept(VisItemPrimaryDown _vis) {
        Primary();
    }
    public override void Accept(VisItemPrimaryUp _vis) { }
    public override void Accept(VisItemReload _vis) { }
    public override void Accept(VisItemGetAmmo _vis) { }
    public override void Accept(VisItemSetAmmo _vis) { }
    public override void Accept(VisItemSetMaxAmmount _vis) { }
    #endregion

    void Primary() {
        RaycastHit _hit;
        if (Physics.Raycast(playerInteraction.cam.transform.position, playerInteraction.cam.transform.forward, out _hit, toolRange, hitMask)) {
            playerInteraction.OnPrimaryToolHit(_hit.collider.gameObject);
        }
    }
}
