using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPartUnloading : MonoBehaviour
{
    public GameObject lastPart;
    
    void Start()
    {
        lastPart.SetActive(true);
    }

    
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            lastPart.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
