using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    public Camera playerCamera;
    void Start()
    {
        
    }

    void Update()
    {
        Ray shotRay = playerCamera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 1000));

        RaycastHit hitPoint;
        if(Physics.Raycast(shotRay, out hitPoint))
        {
            transform.position = hitPoint.point;
        }
    }
}
