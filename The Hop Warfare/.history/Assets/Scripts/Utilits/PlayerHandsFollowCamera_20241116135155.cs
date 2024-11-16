using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandsFollowCamera : MonoBehaviour
{
    public float smoothneses;
    public GameObject cameraObject;
    void Start()
    {
        
    }

    void LateUpdate()
    {
        transform.position = cameraObject.transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, cameraObject.transform.rotation, smoothneses * Time.deltaTime);
    }
}
