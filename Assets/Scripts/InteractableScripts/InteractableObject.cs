using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

    [SerializeField]
    private bool destroyOnInteraction = false;

    public virtual void Interact() {
        
    }
}
