using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image barraImage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AlterarBarra(int VidaAtual, int vidaMaxima){
        barraImage.fillAmount = (float) VidaAtual/vidaMaxima;
    }
}
