using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour {

	// Use this for initialization
	void Start () {
        HasChanged();
	}

    public void HasChanged() {

    }

}

namespace UnityEngine.EventSystems {
    public interface IHasChanged : IEventSystemHandler {
        void HasChanged();
    }
}
