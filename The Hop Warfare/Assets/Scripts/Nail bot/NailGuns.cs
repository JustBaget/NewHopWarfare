using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailGuns : MonoBehaviour
{
    public Transform spawn; // �������, ������ ����� �������� ������
    public GameObject nail; // ������ ������
    public GameObject aim; // ������ ������� (����� ������������ ��� �����������)
    public Transform player; // ������ �� ������
    public float rotationSpeed; // �������� �������� (���� �����)
    public float shootingDistance = 10f; // ������������ ���������� ��� ��������
    public float shootingInterval = 1f; // �������� ����� ����������
    public bool canShoot = true; // ���� �� � ��������
    public bool isRunningAway; // ������� �� ����

    void Start()
    {

    }

    void Update()
    {
        // ��������� ���������� �� ������
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // ���� ����� ������
        if (distanceToPlayer < shootingDistance && !isRunningAway)
        {
            // ������������ ���� � ������ (����� ������������, ���� ������ ������� �� �������)
            //Vector3 direction = player.position - transform.position;
            //Quaternion lookRotation = Quaternion.LookRotation(direction);
            //transform.rotation = Quaternion.Euler(0, 0, aim.transform.rotation.x);

            // ��������
            if (canShoot)
            {
                canShoot = false; // ��������, ��� �� �������� ���������
                Instantiate(nail, spawn.position, spawn.rotation);
                StartCoroutine(CD(shootingInterval)); // ���� ����� ��������� ���������
            }
        }
    }

    IEnumerator CD(float interval)
    {
        yield return new WaitForSeconds(interval);
        canShoot = true; // ��������� �������� ����� ����� ��������� ��������
    }
}