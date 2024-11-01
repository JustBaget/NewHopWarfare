using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Camera playerCamera;
    public Dash Dash;
    public CharacterController characterController;
    public Rigidbody rb;

    public float walkingSpeed = 8.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 10.0f;
    public float rotationX = 0;
    public float maxVelocity;

    public float lookSpeed = 3.25f;
    public float lookXLimit = 100.0f;

    public float recoilForce;
    public float recoilTime;
    public bool isRecoilEnding;

    public float speedXModifier;

    //переменная публичная, т.к. используется в скрипте Dash
    public Vector3 moveDirection = Vector3.zero;

    //то же, что и с moveDirection
    public float movementDirectionX;
    float movementDirectionY;

    [SerializeField]
    bool canMove = true;
    [SerializeField]
    bool isSlamming = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Dash = GetComponent<Dash>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        float curSpeedX = canMove ? (walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (walkingSpeed) * Input.GetAxis("Horizontal") : 0;

        //Во время рывка движение по вертикали останавливается (В ультракалле тоже)
        if (Dash.isDashing)
        {
            moveDirection.y = 0;
            isSlamming = false;
        }
        movementDirectionY = moveDirection.y;
        movementDirectionX = moveDirection.x;
        moveDirection = (forward * (curSpeedX - speedXModifier)) + (right * curSpeedY);

        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");
        //Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        //rb.AddForce(movement * walkingSpeed);

        //if (rb.velocity.magnitude >= maxVelocity)
        //{
        //    rb.velocity = rb.velocity.normalized * maxVelocity;
        //}

        //Прыжок
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded && !isSlamming)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        //Передвижение
        characterController.Move(moveDirection * Time.deltaTime);

        //Приземление
        if (Input.GetKeyDown(KeyCode.LeftControl) && isSlamming == false && !characterController.isGrounded)
        {
            isSlamming = true;
        }
        if (isSlamming == true)
        {
            moveDirection.y = -75f;
            if (characterController.isGrounded)
            {
                isSlamming = false;
                moveDirection.y = 0;
            }
        }
        //вращение камерой
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    public void StartRecoil()
    {
        StartCoroutine(Recoil());
    }
    private IEnumerator Recoil()
    {
        speedXModifier = 100;
        yield return new WaitForSeconds(recoilTime);
        speedXModifier = 20;
        yield return new WaitForSeconds(recoilTime*2);
        speedXModifier = 5;
        yield return new WaitForSeconds(recoilTime * 2);
        speedXModifier = 0;
    }
}