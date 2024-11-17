using UnityEngine;

public class DoorSingle : MonoBehaviour
{
    public GameObject doorObj;
    public GameObject entryTrigger;
    public GameObject exitTrigger;
    public float animDuration;
    public Vector3 targetPosition;
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            DoorMove();
        }
    }

    void OnTriggerEnter(Collider other)
    {

    }

    void DoorMove()
    {

    }
}
