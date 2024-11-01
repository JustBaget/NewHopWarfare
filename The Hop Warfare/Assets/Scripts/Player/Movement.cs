using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Camera playerCamera;
    public Dash Dash;

    public float walkingSpeed = 8.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 10.0f;
    float rotationX = 0;

    public float lookSpeed = 3.25f;
    public float lookXLimit = 100.0f;

    public CharacterController characterController;

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
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

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
        if (Input.GetKeyDown(KeyCode.Q) && isSlamming == false && !characterController.isGrounded)
        {
            isSlamming = true;
        }
        if (isSlamming == true)
        {
            moveDirection.y = -25f;
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
}