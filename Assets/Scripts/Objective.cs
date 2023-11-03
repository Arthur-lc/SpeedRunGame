using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Objective : MonoBehaviour
{
    private AudioSource winSound;
    private bool hasStarted;

    private void Start() {
       winSound = GetComponent<AudioSource>();
       hasStarted = false;
    }
    void Update()
    {
        if (hasStarted && !winSound.isPlaying)
        {
            string audioClipName = winSound.clip.name;
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            winSound.Play();
            hasStarted = true;
        }
    }
}
