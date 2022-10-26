using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LixoSpawn : MonoBehaviour
{
    public GameObject track0Lixo1;
    public GameObject track1Lixo1;
    public GameObject track0Lixo2;
    public GameObject track1Lixo2;
    public GameObject arvoreTrack0;
    public GameObject arvoreTrack1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Track0Lixo1Destroy()
    {
        track0Lixo1.SetActive(false);
    }

    public void Track1Lixo1Destroy()
    {
        track1Lixo1.SetActive(false);
    }

    public void Track0Lixo2Destroy()
    {
        track0Lixo2.SetActive(false);
    }

    public void Track1Lixo2Destroy()
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
