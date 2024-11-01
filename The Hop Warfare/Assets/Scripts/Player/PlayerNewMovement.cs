using System.Collections;
using UnityEngine;

public class PlayerNewMovement : MonoBehaviour
{
    [Header("Основное движение")]
    public float speed;
    public float smoothness;
    public float jumpForce;

    [Header("Рывок")]
    public float dashForce;
    public float dashCD;
    public float dashDuration;

    [Header("Прочее")]
    public float doubleClickTime;
    public GameObject cameraObj;
    Rigidbody rb;
    Vector3 moveVector;
    bool isOnGround;
    public bool canMove = true;
    bool canDash = true;

    //Ребята ниже нужны для расчета рывка, по возможности игнорируйте их
    float timeWhenClickW;
    float lastClickTimeW;
    float timeWhenClickA;
    float lastClickTimeA;
    float timeWhenClickS;
    float lastClickTimeS;
    float timeWhenClickD;
    float lastClickTimeD;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Jump();
        PlayerInput();
        Dash();
    }

    void FixedUpdate()
    {
       Movement();
    }

    void PlayerInput()
    {
        moveVector = new Vector3(Input.GetAxis("Horizontal"), rb.velocity.y, Input.GetAxis("Vertical"));
        moveVector = Quaternion.Euler(transform.rotation.x, cameraObj.transform.eulerAngles.y, transform.rotation.z) * new Vector3(moveVector.x, 0, moveVector.z);
        if(moveVector.magnitude > 1) { moveVector.Normalize(); } //Проверка, чтобы игрок не бегал быстрее по диагонали
    }

    void Movement()
    {
        //transform.position = Vector3.Lerp(transform.position, transform.position + moveVector * speed * Time.deltaTime, smoothness * Time.deltaTime);
        rb.AddForce(moveVector * speed * Time.deltaTime, ForceMode.VelocityChange);
    }

    void Jump()
    {
        if(Input.GetKey(KeyCode.Space) && isOnGround)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            isOnGround = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag;

        switch(tag)
        {
            case "Ground":
                isOnGround = true;
                break;
        }
    }

    void Dash()
    {
        if(Input.GetKeyDown(KeyCode.W)) //Тут все сложно, но суть в том, что этот код считает время с каждого нажатия кнопки, и если оно больше 0.2с - использует рывок
        {
            lastClickTimeW = Time.time - timeWhenClickW;

            if(lastClickTimeW <= doubleClickTime && lastClickTimeW > 0)
            {
                StartCoroutine(DashDuration(cameraObj.transform.forward));
            }

            timeWhenClickW = Time.time;
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            lastClickTimeA = Time.time - timeWhenClickA;

            if(lastClickTimeA <= doubleClickTime && lastClickTimeA > 0)
            {
                StartCoroutine(DashDuration(-cameraObj.transform.right));
            }

            timeWhenClickA = Time.time;
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            lastClickTimeS = Time.time - timeWhenClickS;

            if(lastClickTimeS <= doubleClickTime && lastClickTimeS > 0)
            {
                StartCoroutine(DashDuration(-cameraObj.transform.forward));
            }

            timeWhenClickS = Time.time;
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            lastClickTimeD = Time.time - timeWhenClickD;

            if(lastClickTimeD <= doubleClickTime && lastClickTimeD > 0)
            {
                StartCoroutine(DashDuration(cameraObj.transform.right));
            }

            timeWhenClickD = Time.time;
        }
    }

    IEnumerator DashDuration(Vector3 dashDirection)
    {
        if(canDash)
        {
            StartCoroutine(DashCD());
            rb.velocity = Vector3.zero;
            rb.AddForce(dashDirection * dashForce, ForceMode.VelocityChange);
            yield return new WaitForSeconds(dashDuration);
            rb.velocity = Vector3.zero;
        }
    }

    IEnumerator DashCD()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCD);
        canDash = true;
    }
}