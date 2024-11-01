using UnityEngine;

public class WeaponsFollow : MonoBehaviour
{
    public float speed;
    public float smooth;
    public float handsSmooth;
    Vector2 cursorPos = Vector2.zero;
    Vector2 cursorCurrentPos;

    void Start()
    {
        
    }

    void Update()
    {
        cursorPos = new Vector2(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X")) * speed * Time.deltaTime;
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

        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(cursorCurrentPos.x, cursorCurrentPos.y, 0), handsSmooth * Time.deltaTime);
    }
}
