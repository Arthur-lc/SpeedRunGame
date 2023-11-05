using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Tela de GameOver, com opções de jogar novamente e sair
    public void Play() {
        Debug.Log("Play");
        SceneManager.LoadScene(1);
    }

    public void Exit() {
        Debug.Log("Quit");
        Application.Quit();
    }
}
