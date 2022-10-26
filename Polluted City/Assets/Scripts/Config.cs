using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Config : MonoBehaviour
{
    public GameObject config;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConfigYes()
    {
        config.SetActive(true);
    }

    public void ConfigNot()
    {
        config.SetActive(false);
    }
}
