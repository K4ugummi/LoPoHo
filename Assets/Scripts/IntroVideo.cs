using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroVideo : MonoBehaviour {

    AsyncOperation async;

    void Start() {
        Util.HideCursor();
        StartCoroutine(WaitAndLoad(7.7f));
        async = SceneManager.LoadSceneAsync("LoginMenu");
        async.allowSceneActivation = false;
    }

    void Update() {
        if (Input.anyKey) {
            async.allowSceneActivation = true;
        }
    }

    IEnumerator WaitAndLoad(float value) {
        yield return new WaitForSeconds(value);
        async.allowSceneActivation = true;
    }
}