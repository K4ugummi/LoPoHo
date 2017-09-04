using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroVideo : MonoBehaviour {

    void Start() {
        StartCoroutine(WaitAndLoad(7.7f, "LoginMenu"));
    }

    void Update() {
        if (Input.anyKey) {
            SceneManager.LoadScene("LoginMenu");
        }
    }

    IEnumerator WaitAndLoad(float value, string scene) {
        yield return new WaitForSeconds(value);
        SceneManager.LoadScene(scene);
    }
}