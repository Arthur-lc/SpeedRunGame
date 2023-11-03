using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private int VidaAtual;
    private int vidaTotal = 10;
    private Animator anim;

    [SerializeField] private HealthBar barra;
    void Start()
    {
        anim = GetComponent<Animator>();
        VidaAtual = vidaTotal;
        barra.AlterarBarra(VidaAtual,vidaTotal);
    }

    // Update is called once per frame
    void Update()
    {
        if(VidaAtual <= 0){
            Die();
        }
    }
    public void Dano(int dano){
        VidaAtual -= dano;
        barra.AlterarBarra(VidaAtual,vidaTotal);
    }
    public void Die(){
        anim.SetBool("hit",true);
        StartCoroutine(ScaleDownOverTime(2.0f));
    }
    private IEnumerator ScaleDownOverTime(float duration)
    {
        float elapsedTime = 0f;
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = Vector3.zero; 

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
        SceneManager.LoadScene("GameOver");
    }
}
