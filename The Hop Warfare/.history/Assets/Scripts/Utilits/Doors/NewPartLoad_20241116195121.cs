using UnityEngine;

public class NewPartLoad : MonoBehaviour
{
    public GameObject nextPart;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            nextPart.SetActive = true;
        }
    }
}