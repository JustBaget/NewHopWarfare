using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorSingle : MonoBehaviour
{
    public GameObject doorObj;
    public float animDuration;
    public Transform targetPosition;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            doorObj.transform.position = 
        }
    }
}
