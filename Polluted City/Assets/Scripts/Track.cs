using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public GameObject[] obstacles; // variavel para referenciar os obstaculos la na unity
    public Vector2 numberOfObstacles; // quantidade de obstaculos, fazendo um sorteio sobre esses valores, no caso em posições
    public List<GameObject> newObstacles; // criando a lista de obstaculos, referenciado no unity
    public GameObject coin; // variavel referente ao gameobject das moedas
    public Vector2 numberOfCoins; //numero minimo e max de moedas que podem ser instanciadas
    public List<GameObject> newCoins; // lista para colocar a moeda
    public GameObject track0Lixo1;
    public GameObject track1Lixo1;
    public GameObject track0Lixo2;
    public GameObject track1Lixo2;
    public GameObject arvoreTrack0;
    public GameObject arvoreTrack1;
    private Player playerScore;
    bool lixo0Destruido = false;
    bool lixo1Destruido = false;
    bool lixo2Destruido = false;
    bool lixo3Destruido = false;
    bool arvore0Track = false;
    bool arvore1Track = false;



    void Start()
    {
        playerScore = FindObjectOfType<Player>();
        int newNumberOfObstacles = (int)Random.Range(numberOfObstacles.x , numberOfObstacles.y); // aqui eu criei o sorteio para os obstaculos que serao criados
        int newNumberOfCoins = (int)Random.Range(numberOfCoins.x, numberOfCoins.y); //sorteio entre as moedas que poderam ser invocadas

        for (int i = 0; i < newNumberOfObstacles; i++)
        {
            newObstacles.Add(Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform)); //adicionando os obstaculos na lista para ser sorteada e instanciada
            newObstacles[i].SetActive(false);
        }

        for (var i = 0; i < newNumberOfCoins; i++) // vamos de 0 até o total de moedas que colocamos na lista
        {
            newCoins.Add(Instantiate(coin, transform)); // adicionando o prefab nas moedas
            newCoins[i].SetActive(false); //

        }

        PositionateCoins(); // chamando a função das moedas
        PositionateObstacles(); // chamando a função para os obstaculos
    }

    void Update()
    {
        if(playerScore.score >= 310f)
        {
            if ( lixo0Destruido == false ) {
                 Track0Lixo1Destroy();
                 lixo0Destruido = true;
            }
        }

        if(playerScore.score >= 610f)
        {
            if ( lixo1Destruido == false ) {
                 Track1Lixo1Destroy();
                 lixo1Destruido = true;
            }            
        }

        if(playerScore.score >= 910f)
        {
            if ( lixo2Destruido == false ) {
                 Track0Lixo2Destroy();
                 ArvoreTrack0();
                 lixo2Destruido = true;
            }                   
        }

        if(playerScore.score >= 1210f)
        {
            if ( lixo3Destruido == false ) {
                 Track1Lixo2Destroy();
                 ArvoreTrack1();
                 lixo3Destruido = true;
            }                   
        }
    }

    void PositionateObstacles() // função para posicionar os obstaculos
    {
        for (var i = 0; i < newObstacles.Count; i++) //contagem para as direções
        {
            float posZMin = (297f / newObstacles.Count) + (297 / newObstacles.Count) * i; // posição minima no Z 
            float posZMax = (297f / newObstacles.Count) + (297 / newObstacles.Count) * i + 1;
            newObstacles[i].transform.localPosition = new Vector3(0, 0, Random.Range(posZMin , posZMax)); // definindo o local que os obstaculos vao ser sorteados e sorteando com base no posição minima e maxima
            newObstacles[i].SetActive(true);
            if(newObstacles[i].GetComponent<ChangeLane>() !=null) //verificando se eles tem o componente, pois só vamos usar o change lane na lixeira
                newObstacles[i].GetComponent<ChangeLane>().PositionLane();
        }
    }

    void PositionateCoins()
	{
		float minZPos = 10f; // posição minima da primeira moeda em z
		for (int i = 0; i < newCoins.Count; i++) // o loop vai de 0 até o tamanho da lista
		{
			float maxZPos = minZPos + 5f; // posição maxima que a moeda pode nascer em z
			float randomZPos = Random.Range(minZPos, maxZPos); //fazendo o random entre o minimo e max na posição z
			newCoins[i].transform.localPosition = new Vector3(transform.position.x, transform.position.y, randomZPos); //pegamos a moeda atual da lista e posição dela, e passamos um vetor 3 mudando apenas o Z pra random
			newCoins[i].SetActive(true); // depois pegamos ela e ativamos
			newCoins[i].GetComponent<ChangeLane>().PositionLane(); //chamando a função do script das lanes
			minZPos = randomZPos + 1; // atualizando o valor minimo da posição z, ou seja, a prox moeda no minimo vai ter valor de 1 em z da anterior, pra nao ficar muito perto
		}
	}

    private void OnTriggerEnter(Collider other) // usar essa função para que assim que o jgador colidir com o collider no fim da pista, ela sera movida para frente
    {
        if(other.CompareTag("Player")) // usei o compare tag pois sfica mais facil de identifcar quando o jogador passar pelo collider
        {
            other.GetComponent<Player>().IncreaseSpeed(); // chamando a função que aumenta a velocidade, de for que ela aumente assim que ele passar certa distancia do mapa
            transform.position = new Vector3(0, 0, transform.position.z + 297 * 2); // posição da pista, eu adiciono valor em vector 3 no valor Z que no caso é 297 multiplicado por 2
            //Invoke ("PositionateObstacles" , 2f); // usando o invoke para ter um atraso na função, e nao cortar mapa
            StartCoroutine(WaitExecute());
        }
    }

    IEnumerator WaitExecute()
    {
        yield return new WaitForSeconds(4f);
        PositionateObstacles();
        PositionateCoins();
    }

    void Track0Lixo1Destroy()
    {
        track0Lixo1.SetActive(false);
    }

    void Track1Lixo1Destroy()
    {
        track1Lixo1.SetActive(false);
    }

    void Track0Lixo2Destroy()
    {
        track0Lixo2.SetActive(false);
    }

    void Track1Lixo2Destroy()
    {
        track1Lixo2.SetActive(false);
    }

    public void ArvoreTrack0()
    {
        arvoreTrack0.SetActive(true);
    }

    public void ArvoreTrack1()
    {
        arvoreTrack1.SetActive(true);
    }
}
