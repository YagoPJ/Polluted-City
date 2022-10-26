using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Transform player; //variavel referente ao player que  camera vai seguir
    private Vector3 offSet; //referente a distancia da camera com o player
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // chamando o transform do gameobjetct que tiver a tag player
        offSet = transform.position - player.position; // criando uma logica 
    }

    void LateUpdate() // late update chama depois que o frame termina, só pra garantir que a camera vai seguir o player corretamente
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, player.position.z + offSet.z); // aplicando a posição da camera
        transform.position = newPosition; // e passando o valor pro transform.
    }
}
