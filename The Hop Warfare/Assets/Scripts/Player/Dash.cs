using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    Movement Movement;

    public float dashSpeed;
    public float dashTime;

    [HideInInspector]
    public bool isDashing;
    void Start()
    {
        Movement = GetComponent<Movement>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(StartDash());
        }
    }
    IEnumerator StartDash()
    {
        isDashing = true;
        float startTime = Time.time;

        while(Time.time < startTime + dashTime)
        {
            Movement.characterController.Move(Movement.moveDirection * dashSpeed * Time.deltaTime);

            yield return null;
        }
        isDashing = false;
    }
}
