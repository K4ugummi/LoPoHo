using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragSlot : MonoBehaviour, IDropHandler {

    [SerializeField]
    public int slotID;
    // slotType
    // 0: Selectable Items;
    // 1: Inventory;
    // 2: External;
    [SerializeField]
    public int slotTypeID;
    [HideInInspector]
    public string dragSlotHint;

    public GameObject item {
        get {
            if (transform.childCount > 0) {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData _eventData) {
        if (!item) {
            DragHandler.itemBeingDragged.transform.SetParent(transform);
            int _slotID = transform.GetComponent<DragSlot>().slotID;
            int _slotTypeID = transform.GetComponent<DragSlot>().slotTypeID;

            GUIPlayer _gui = GetComponentInParent<GUIPlayer>();
            Debug.Log("SWITCHING: " + DragHandler.startID + " " + DragHandler.startTypeID + " to " + _slotID + " " + _slotTypeID);
            ItemSwitchInfo _switchInfo = new ItemSwitchInfo(DragHandler.startID, DragHandler.startTypeID, _slotID, _slotTypeID);
            _gui.ProcessSwitchedItems(_switchInfo);
            
        }
    }

}
