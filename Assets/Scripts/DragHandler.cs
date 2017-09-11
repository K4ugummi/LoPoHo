using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    public static int startID;
    public static int startTypeID;
    Transform startParent;
    bool start = true;

    public void OnBeginDrag(PointerEventData _eventData) {
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent.transform;
        startID = transform.parent.transform.GetComponent<DragSlot>().slotID;
        startTypeID = transform.parent.transform.GetComponent<DragSlot>().slotTypeID;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        itemBeingDragged.GetComponent<LayoutElement>().ignoreLayout = true;
        itemBeingDragged.transform.SetParent(itemBeingDragged.transform.parent.parent);
    }

    public void OnDrag(PointerEventData _eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData _eventData) {
        if (transform.parent == startParent) {
            transform.position = startPosition;
        }
        if (transform.parent.GetComponent<DragSlot>() == null) {
            transform.parent = startParent;
            transform.position = startPosition;
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        itemBeingDragged.GetComponent<LayoutElement>().ignoreLayout = false;
        itemBeingDragged = null;
    }

}
