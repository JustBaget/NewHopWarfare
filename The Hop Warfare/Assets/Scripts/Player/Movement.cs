using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Другие скрипты/объекты")]

    public Camera playerCamera;
    public CharacterController characterController;

    [Header("Основное")]
    public float walkingSpeed = 8.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 10.0f;

    [Header("Камера")]
    public float lookSpeed = 3.25f;
    public float lookXLimit = 100.0f;
    private float rotationX = 0;

    [Header("Рывок")]
    public float dashSpeed;
    public float dashTime;
    public float cooldown;

    [Header("Слэм/резкое приземление")]
    public float slamSpeed;

    [Header("Датчики")]
    public bool isOnGround;
    public bool canMove = true;
    public bool canDash = true;

    private float speedXModifier;
    private float speedYModifier;

    [HideInInspector]
    //переменная публичная, т.к. используется в скрипте Dash
    public Vector3 moveDirection = Vector3.zero;

    [HideInInspector]
    //то же, что и с moveDirection
    public float movementDirectionX;

    bool isRecoilActive;
    float movementDirectionY;
    bool isDashing;
    float cameraRotation;
    bool isSlamming = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        float curSpeedX = canMove ? (walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (walkingSpeed) * Input.GetAxis("Horizontal") : 0;

        isOnGround = characterController.isGrounded;

        //Во время рывка движение по вертикали останавливается (В ультракалле тоже)
        if (isDashing)
        {
            moveDirection.y = 0;
            isSlamming = false;
        }
        if (!isRecoilActive)
        {
            movementDirectionY = moveDirection.y;
        }
        else
        {
            movementDirectionY = speedYModifier;
        }
        movementDirectionX = moveDirection.x;
        moveDirection = (forward * (curSpeedX - speedXModifier)) + (right * curSpeedY);

        cameraRotation = playerCamera.transform.localRotation.x * 20;

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
            moveDirection.y = -(slamSpeed);
            if (characterController.isGrounded)
            {
                isSlamming = false;
                moveDirection.y = 0;
            }
        }

        speedXModifier = Mathf.Lerp(speedXModifier, 0, 3 * Time.deltaTime);
        speedYModifier = Mathf.Lerp(speedYModifier, 0, 0.5f * Time.deltaTime);

        //вращение камерой
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        //Рывок
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash == true)
        {
            canDash = false;
            StartCoroutine(StartDash());
            StartCoroutine(DashCooldown());
        }
    }

    IEnumerator StartDash()
    {
        isDashing = true;
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            characterController.Move(moveDirection * dashSpeed * Time.deltaTime);

            yield return null;
        }
        isDashing = false;
    }
    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canDash = true;
    }

    //Отдача дробовика
    public void StartRecoil(float force, float verticalForce, float cooldown)
    {
        StartCoroutine(Recoil(force, verticalForce, cooldown));
    }
    public IEnumerator Recoil(float force, float verticalForce, float cooldown)
    {
        isRecoilActive = true;
        speedXModifier = force - (cameraRotation * 3);
        speedYModifier = cameraRotation * verticalForce;
        yield return new WaitForSeconds(cooldown);
        //speedXModifier = 0;
        //speedYModifier = 0;
        isRecoilActive = false;
    }
}