using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorSingle : MonoBehaviour
{
    public GameObject doorObj;
    public float animDuration;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag<"Player")
        {
            
        }
    }
}
