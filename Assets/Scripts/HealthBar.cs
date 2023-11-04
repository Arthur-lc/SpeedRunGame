using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Componente de vida em cima do personagem, com uma função que altera a vida do personagem de acordo com a vida atual
    [SerializeField] private Image barraImage;
    public void AlterarBarra(int VidaAtual, int vidaMaxima){
        barraImage.fillAmount = (float) VidaAtual/vidaMaxima;
    }
}
