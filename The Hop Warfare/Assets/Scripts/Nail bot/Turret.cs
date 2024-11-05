using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject aim;
    public float rotationSpeed;
    public NailGuns gun;
    public float shootAngle;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, aim.transform.eulerAngles.y - 90, 0), rotationSpeed * Time.deltaTime);
    }
}
