using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject aim;
    public float rotationSpeed;
    public GameObject gun;
    public float angle;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, aim.transform.eulerAngles.y - 90, 0), rotationSpeed * Time.deltaTime);
        if (gun.transform.localRotation.z <= 45 && gun.transform.localRotation.z >= -45)
        {
            gun.transform.localRotation = Quaternion.Euler(0, 180, aim.transform.eulerAngles.x - 180);
        }

        angle = aim.transform.eulerAngles.x - 180;
    }
}
