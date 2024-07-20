using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    void Start() {
        Pause.paused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void PlayGame() {
        SceneManager.LoadScene(1);
    }
    public void Options() {

    }
    public void QuitGame() {
        Application.Quit();
    }
}
