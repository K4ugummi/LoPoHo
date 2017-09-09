using UnityEngine;
using System.Net;
using System.IO;

public class Util {

    private const int CURRENT_VERSION = 170909;

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

    public static void TakeScreenshot() {
        string _dateTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        ScreenCapture.CaptureScreenshot(Application.dataPath + "screenshot_" + _dateTime + ".png");
    }

    public static bool CheckNewVersionOnline() {
        WebClient _client = new WebClient();
        Stream _stream = _client.OpenRead("http://schauerte.online/version.txt");
        StreamReader _reader = new StreamReader(_stream);
        string _content = _reader.ReadToEnd();
        int _version;
        int.TryParse(_content, out _version);
        if (_version != CURRENT_VERSION) {
            return false;
        }
        else {
            return true;
        }
    }
}
