using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Menu para jogar ou sair do jogo
    public void Play() {
        Debug.Log("Play");
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void Exit() {
        Debug.Log("Quit");
        Application.Quit();
    }
}
