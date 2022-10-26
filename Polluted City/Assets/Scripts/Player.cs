using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb; //referenciando o componente rigidbody
    private Vector3 verticalTargetPosition;
    private Vector3 boxColliderSize; // variavel que vou usar para ter o controle do tamanho do box collider
    private BoxCollider boxCollider; // referenciando o component boxcollider la da unity
    private float slideStart; // variavel para começo do escorre
    private bool sliding = false; // verificando se ele ja nao esta escorregando
    private Animator anim; // variavel referente ao animator, para usarmos as animações
    private float jumpStart; // variavel para saber qual distancia ele começou o pulo
    private bool jumping = false; // criei essa variavel para ver se o player ja está no pulo, para ele nao conseguir pular infinitas vezes
    private int currentLane = 1; // valor da lane atual é um, ou seja, vou usar 3 lanes 0 1 2 onde 0 é esquerda 1 meio 2 direita
    private int currentLife; // vida atual do jogador
    private bool invincible = false;
    private bool canMove; //vou usar ela pra bloquear o personagem de ir pros lados quando tiver morto
    static int blinkingValue; // valor do blink
    private UIManager uiManager; // variavel para referenciar o script
    [HideInInspector]
    public int coins; // para contabilizar as coins
    // esconder o score do unity
    public float score;  //variavel referente ao score
    public float speed; // referenciando o valor para multiplicar a velocidade
    public float laneSpeed; // variavel para a velocidade de mudança de lane
    public float jumpLenght; // distancia total do nosso pulo
    public float jumpHeight; //altura do pulo
    public float slideLenght; // distancia do escorrega
    public int maxLife = 3; // variavel referente ao valor de vida maxima dele
    public float minSpeed = 10f; // velocidade minima que o jogador corre
    public float maxSpeed = 30f; // maxima que ele corre
    public float invincibleTime; //tempo de invencibilidade
    public GameObject model; //modelo que vamos ativar e desativar para fazer o efeito invencivel
    public AudioSource jumpEfects;
    public AudioSource slideEfects;

    void Start()
    {
        jumpEfects = GameObject.FindGameObjectWithTag("JumpEfect").GetComponent<AudioSource>();
        slideEfects = GameObject.FindGameObjectWithTag("SomSlide").GetComponent<AudioSource>();
        canMove = false;
        rb = GetComponent<Rigidbody>(); // referenciando que o rb que criamos é o rb do player
        anim = GetComponentInChildren<Animator>(); // referenciando a variavel anim ao componente animator la no unity, usei o inchildren pois o animator esta no filho do gameobject que o script ta, no caso o character
        boxCollider = GetComponent<BoxCollider>(); // referenciando a variavel ao component
        boxColliderSize = boxCollider.size; // guardando o tamanho inicial do collider na variavel
        currentLife = maxLife; // assim que o jogo começar, a vida atual do jogador é igual a vida maxima
        blinkingValue = Shader.PropertyToID("_BlinkingValue"); //passando o shader de invulnerabilidade pro blinkingValue
        uiManager = FindObjectOfType<UIManager>(); // referencia a variavel ao objeto UIManager
        GameManager.gm.StartMissions();

        Invoke("StartRun", 2f); // chamando a corrida depois de 2 segundos

    }

   
    void Update()
    {
        if(!canMove)
            return;
        score += Time.deltaTime * speed; // aumentando o score a todo tempo de acordo com a velocidade
        uiManager.UpdateScore((int) score); // aplicando o score no text
        if(Input.GetKeyDown(KeyCode.LeftArrow))//comando para ir para esquerda
        {
            ChangeLane(-1);// chamando a função criada la embaixo
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))//ir para a direita
        {
            ChangeLane(1);//chamando a função criada la embaixo
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))//criando o pulo
        {
            Jump();//chamando a função pulo
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            Slide();
        }

        if(jumping) // se jumping for true
        {
            float ratio = (transform.position.z - jumpStart) / jumpLenght; // controlando a proporção do pulo, quando for maior que 1 ele acaba, ou seja um limite
            if(ratio >= 1) // maior ou = 1 ele desligar o pulo
            {
                jumping = false; // desligando o pulo
                anim.SetBool("Jumping", false); //atualizando o animator para desligar o pulo
            }
            else
            {
                verticalTargetPosition.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeight; // aqui fazemos o pulo, atualizando a posição em Y, o mthf e sin 
            }
        }
        else // se não estiver em pulo
        {
            verticalTargetPosition.y = Mathf.MoveTowards(verticalTargetPosition.y, 0, 5 * Time.deltaTime); //como nao estou usando a gravidade, eu mesmo vou puxar ele para baixo
        }

        if(sliding) //se estiver escorregando
        {
            float ratio = (transform.position.z - slideStart) / slideLenght; // definindo um fim para o escorrega
            if(ratio >= 1) // valor para o fim
            {
                sliding = false; //desativando o escorrega
                anim.SetBool("Sliding", false); // desativando a animação
                boxCollider.size = boxColliderSize; // voltando o colisor pro tamanho normal
            }
        }

        Vector3 targetPosition = new Vector3(verticalTargetPosition.x, verticalTargetPosition.y, transform.position.z); // atualizando a posição, com a posição algo targetposition e mudamos apenas em z, pois só estamos correndo, e em y fica para depois no pulo
        transform.position = Vector3.MoveTowards(transform.position, targetPosition,laneSpeed * Time.deltaTime); // aqui atualizamos nossa posição, utilizando a função move towards que é( posição atual, posição alvo, e a velocidade para vc ir ate o alvo)

    }

    private void FixedUpdate()//usar a função fixed pq ela é usada em cada tempo fixo ao contrario do update que é por frame, entao em maquinas inferiores isso ajuda a nao ocorrer bugs
    {
        rb.velocity = Vector3.forward * speed;//dessa forma ele ja anda pra frente, no caso o Vector 3 fornece 3 direções e o forward diz que é pra ele ir pra frente vezes a velocidade (speed)
    }

    void StartRun() // função que vou usar o invoke, pra quando começar o jogo o personagem ficar parado e depois correr
    {
        anim.Play("runStart");
        speed = minSpeed; // velocidade padrao
        canMove = true;
    }

    void ChangeLane(int direction)//passando o parametro para direção
    {
        int targetLane = currentLane + direction; // posição algo é = a lane atual + a direção
        if(targetLane < 0 || targetLane > 2)
            return; //aqui nao vamos permitir que a lane alvo seja maior que 2 ou menor do que 0, ou seja, impus um limite de movimentação
        currentLane = targetLane;
        verticalTargetPosition = new Vector3((currentLane - 1), 0, 0); //tiramos 1 de current lane, pois o personagem começa em 0 e nossa lane começa em 1 então tiramos para começar no 0
    }

    void Jump()
    {
        if((!jumping))//se o jogador nao estiver pulando
        {
            jumpEfects.Play();
            jumpStart = transform.position.z; // passando valor ao jumpstart
            anim.SetFloat("JumpSpeed", speed / jumpLenght); //executando a animação que está la no animator, e dividindo o valor de speed por jumplenght
            anim.SetBool("Jumping", true); //executando a animação la do animator
            jumping = true; // variavel que controla se eu estou pulando ou nao, passa pra verdadeiro, pois vou executar o pulo
        }
    }

    void Slide() // criando a função para escorregar
    {
        if(!jumping && !sliding) // se ele nao estiver pulando nem escorregando..
        {
            slideEfects.Play();
            slideStart = transform.position.z; // definindo a variavel start sobre a posição
            anim.SetFloat("JumpSpeed", speed / slideLenght); // ativando a animação e modificando
            anim.SetBool("Sliding", true); // ativando a animação
            Vector3 newSize = boxCollider.size; // utilizando o vector 3 para definir e depois alterar o tamanho do boxcollider, pois nao podemos modificar ele diretamente
            newSize.y = newSize.y / 2; // dividindo o newsyze por 2 para usar ele na linha debaixo
            boxCollider.size = newSize; //dividimos o boxcollider por 2
            sliding = true; // estamos escorregando
        }
    }

    private void OnTriggerEnter(Collider other) // criando a função para tomar dano
	{
        if(other.CompareTag("Coin")) // analisando se o player colidiu com a coin
        {
            coins++; // se colidiu adiciona + 1 a coins
            uiManager.UpdateCoins(coins); // ele joga o valor na função criada em UIManager
            other.transform.parent.gameObject.SetActive(false); //e desativa a moeda para ela sair do mapa, desativando o filho.
        }
        if(invincible) //se ele estiver invencivel, ele retorna da função, para nao tomar outro dano
        {
            return;
        }
		if (other.CompareTag("Obstacle")) // comparando a tag, para usar o colisor
		{
            canMove = false;
            currentLife --; //diminuir de 1 em 1 a vida atual
            uiManager.UpdateLives(currentLife); //passando a função para atualizar as vidas
            anim.SetTrigger("Hit"); //passando a animação de dano
            speed = 0;
            if(currentLife <= 0) // se as vidas acabarem
            {
                uiManager.somGameOver.SetActive(false);
                speed = 0; // parando o personagem
                anim.SetBool("Dead", true); //animação de morte
                uiManager.gameOverPanel.SetActive(true); // chamando o painel de game over
                Invoke("CallMenu", 2f); //Aqui chamamos a função que puxa a cena, e usamos o invoke pra demorar 2 segundos para essa cena ser chamada

            }
            else // fazer o player piscar, e deixar invulneravel
            {
                Invoke("CanMove", 0.75f);
                StartCoroutine(Blinking(invincibleTime)); //se ele ainda tiver vida sobrand, ele vem pra ca inves do game over
            }
		}
	}

    void CanMove()
    {
        canMove = true;
    }

    IEnumerator Blinking(float time) //numerator para fazer o tempo de invulneravel ( blink)
    {
        invincible = true; //passando a variavel bool como verdadeira
        float timer = 0; // cronometro para controla o tempo de invencibilidade
        float currentBlink = 1f; //tempo de invencibilidade
        float lastBlink = 0; //ultimo blink
        float blinkPeriod = 0.1f; //de quanto em quanto tempo ele vai piscar
        bool enabled = false; //usar como ferramenta para ativar e desativar o modelo
        yield return new WaitForSeconds(1f); // esperar um tempo para continuar
        speed = minSpeed; // a velocidade atual vai pra velocidade minima
        while(timer < time && invincible)
        {
            model.SetActive(enabled); // se enabled for true ele vai ativar o modelo
            //Shader.SetGlobalFloat(blinkingValue, currentBlink); // puxando o shader
            yield return null; // retornando nulo
            timer += Time.deltaTime; // atualizando o time
            lastBlink += Time.deltaTime; //atualizando o ultimo blink
            if(blinkPeriod < lastBlink) //encerramos os blink 
            {
                lastBlink = 0;
                currentBlink = 1f - currentBlink;
                enabled = !enabled; // e nisso ele fica ativando e desativando
            }

        }

        model.SetActive(true);
        //Shader.SetGlobalFloat(blinkingValue, 0); // voltando ao estado padrao do modelo
        invincible = false; //desligando o invencible
    }

    void CallMenu() // função separada para chamar as cenas
    {
        GameManager.gm.coins += coins;
        GameManager.gm.EndRun(); //cena menu
        Cursor.visible = true;
    }

    public void IncreaseSpeed() // função para aumentar a velocidade com o tempo
    {
        speed *= 1.15f; // aumentando em 15%
        if(speed >= maxSpeed)
            speed = maxSpeed; //limitando a velocidade ao maxspeed definido
    }

}
