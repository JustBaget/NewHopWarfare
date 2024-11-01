using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("������ �������/�������")]

    public Camera playerCamera;
    public CharacterController characterController;

    [Header("������")]
    public float walkingSpeed = 8.0f;

    [Header("������")]
    public float jumpSpeed = 8.0f;
    public float gravity = 10.0f;

    [Header("�������� �������")]
    public float lookSpeed = 3.25f;
    public float lookXLimit = 100.0f;
    private float rotationX = 0;

    [Header("�����")]
    public float dashSpeed;
    public float dashTime;

    [Header("����/������ �����������")]
    public float slamSpeed;

    [Header("������ ���������")]
    public float recoilForce;
    public float recoilTime;
    public float verticalRecoil;

    private bool isRecoilActive;

    private float speedXModifier;
    private float speedYModifier;

    [Header("�������� ������ ���������� (�� �������� � ���������)")]
    //���������� ���������, �.�. ������������ � ������� Dash
    public Vector3 moveDirection = Vector3.zero;

    //�� ��, ��� � � moveDirection
    public float movementDirectionX;
    float movementDirectionY;

    public bool isDashing;

    [SerializeField]
    float cameraRotation;

    [SerializeField]
    bool canMove = true;
    [SerializeField]
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

        //�� ����� ����� �������� �� ��������� ��������������� (� ����������� ����)
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

        //������
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

        //������������
        characterController.Move(moveDirection * Time.deltaTime);

        //�����������
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
        //�������� �������
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        //�����
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(StartDash());
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

    //������ ���������
    public void StartRecoil()
    {
        StartCoroutine(Recoil());
    }
    private IEnumerator Recoil()
    {
        isRecoilActive = true;
        speedXModifier = recoilForce - (cameraRotation * 3);
        speedYModifier = cameraRotation * verticalRecoil;
        yield return new WaitForSeconds(recoilTime);
        speedXModifier = 0;
        speedYModifier = 0;
        isRecoilActive = false;
    }
}