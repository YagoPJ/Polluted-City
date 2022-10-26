using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image[] lifeHearths; // variavel para referenciar as imagens de vida que vamos utilizar no canvas
    public Text coinText; //variavel de referencia para o Coin Text do canvas no unity
    public GameObject gameOverPanel; //referenciar o painel do game over
    public Text scoreText; //referente ao texto score la da unity

    public GameObject somGameOver;

    public void UpdateLives(int lives) // função para desativar os corações conforme vc perde vida
    {
        for (int i = 0; i < lifeHearths.Length; i++) // fazendo o loop com a quantidade de vidas
        {
            if(lives > i) 
            {
                lifeHearths[i].color = Color.white; //se a vida for maior que o index (3 de vida) o coração continua normal
            }
            else
            {
                lifeHearths[i].color = Color.black; // se for menor, ele passa pra preto, ele desativa
            }

        }
    }

    public void UpdateCoins(int coin) // função para atualizar as coins
	{
		coinText.text = coin.ToString();
	}

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score + "m";
    }
}
