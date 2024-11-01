using System.Collections;
using UnityEngine;

public class CameraFPV : MonoBehaviour
{
    [Header("Camera Rotation")]
    public float cameraSpeed;
    public float cameraSmooth;
    Vector2 cursorPos = Vector2.zero;
    Vector2 cursorCurrentPos;
    Quaternion cameraRotation;

    [Header("Camera Fov")]
    public float FOVChangeSpeed;
    public float startFOV;
    float FOV;
    public float maxFOV;
    public GameObject player;
    Camera cameraObj;
    float currentVelocity;
    Rigidbody rb;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = player.GetComponent<Rigidbody>();
        FOV = GetComponent<Camera>().fieldOfView;
        cameraObj = GetComponent<Camera>();
        FOV = startFOV;
    }

    void LateUpdate()
    {
        FieldOfViev();
        CameraRotation();
    }

    void CameraRotation()
    {
        cursorPos = new Vector2(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X")) * cameraSpeed * Time.deltaTime;
        cursorCurrentPos -= cursorPos;
        cursorCurrentPos.x = Mathf.Clamp(cursorCurrentPos.x, -90, 90);
        cursorCurrentPos.y = Mathf.Clamp(cursorCurrentPos.y, -180, 180);

        switch (cursorCurrentPos.y)
        {
            case >= 180:
                cursorCurrentPos.y *= -1;
                break;

            case <= -180:
                cursorCurrentPos.y *= -1;
                break;
        }

        Quaternion cameraRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(cursorCurrentPos.x, cursorCurrentPos.y, 0), cameraSmooth * Time.deltaTime);
        transform.localRotation = cameraRotation;
    }

    void FieldOfViev()
    {
        FOV = Mathf.Lerp(FOV, startFOV + rb.velocity.magnitude, FOVChangeSpeed * Time.deltaTime);
        FOV = Mathf.Clamp(FOV, startFOV, maxFOV);
        cameraObj.fieldOfView = FOV;
    }

    public IEnumerator InstantShake(float strength, int duration)
    {   
        for (int i = 0; i < duration; i++)
        {
            Vector3 shakePos = new Vector3(cursorCurrentPos.x + Random.Range(-strength, strength), //перенос
            cursorCurrentPos.y + Random.Range(-strength, strength), transform.localRotation.z + Random.Range(-strength, strength));
            transform.localRotation = Quaternion.Euler(shakePos.x, shakePos.y, shakePos.z);
            yield return new WaitForSeconds(0.005f);
        }

        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(cursorCurrentPos.x, cursorCurrentPos.y, 0), cameraSmooth * Time.deltaTime);
    }

    public IEnumerator ShakeLong(float strength, bool isShaking)
    {
        while(isShaking)
        {
            Vector3 shakePos = new Vector3(cursorCurrentPos.x + Random.Range(-strength, strength), //перенос
            cursorCurrentPos.y + Random.Range(-strength, strength), transform.localRotation.z + Random.Range(-strength, strength));
            transform.localRotation = Quaternion.Euler(shakePos.x, shakePos.y, shakePos.z);
            yield return new WaitForSeconds(0.01f);
        }

        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(cursorCurrentPos.x, cursorCurrentPos.y, 0), cameraSmooth * Time.deltaTime);
    }
}