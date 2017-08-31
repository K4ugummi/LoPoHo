using UnityEngine;

public class Util {

	public static void SetLayerRecursively(GameObject _obj, int _newLayer) {
        if (_obj == null) {
            return;
        }

		_obj.layer = _newLayer;
		foreach (Transform _child in _obj.transform) {
            if (_child == null) {
                continue;
            }
			SetLayerRecursively(_child.gameObject, _newLayer);
		}
	}

    // _state: - true: Hide cursor
    //         - false: Show cursor 
    public static void HideCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void ShowCursor() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public static float GetValueInPct(float _amount, float _max) {
        return _amount / _max;
    }
}
