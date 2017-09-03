using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroVideo : MonoBehaviour {

    private void Start() {
        StartCoroutine(WaitAndLoad(7.5f, "LoginMenu"));
    }

    private IEnumerator WaitAndLoad(float value, string scene) {
        yield return new WaitForSeconds(value);
        SceneManager.LoadScene(scene);
    }
}