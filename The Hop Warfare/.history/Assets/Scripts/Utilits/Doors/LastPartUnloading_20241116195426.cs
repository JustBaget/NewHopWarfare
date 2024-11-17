using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPartUnloading : MonoBehaviour
{
    public GameObject lastPart;
    
    void Start()
    {
        nextPart.SetActive(false);
    }

    
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            nextPart.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
