using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ItemImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    
    [SerializeField]
    TMP_Text tmpHint;
    [SerializeField]
    string objectNameOrHint;

    void Start() {
        tmpHint.gameObject.SetActive(false);
        tmpHint.text = objectNameOrHint;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Debug.Log("Mouse enter");
        StartCoroutine("ShowHint");
    }

    public void OnPointerExit(PointerEventData eventData) {
        Debug.Log("Mouse exit");
        StopCoroutine("ShowHint");
        tmpHint.gameObject.SetActive(false);
    }

    IEnumerator ShowHint() {
        yield return new WaitForSeconds(0.5f);
        tmpHint.gameObject.SetActive(true);
    }
}
