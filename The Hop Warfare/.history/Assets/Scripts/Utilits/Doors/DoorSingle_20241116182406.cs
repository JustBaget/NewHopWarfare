using UnityEngine;

public class DoorSingle : MonoBehaviour
{
    public GameObject doorObj;
    public float animDuration;
    public Vector3 targetPosition;
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
            gameObject.SetActive(false);
            doorObj.SetActive(false);
            doorObj.transform.position = Vector3.Lerp(doorObj.transform.position, targetPosition, animDuration * Time.deltaTime);
            Debug.Log("Дверь пройдена");
        }
    }
}
