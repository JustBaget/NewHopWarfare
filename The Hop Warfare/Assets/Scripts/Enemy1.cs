using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    float distance;
    PlayerHealth playerHealth;

    public float attackDistance;
    public float damage;
    public GameObject player;
    void Start()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

        if(attackDistance > distance)
        {
            playerHealth.Damage(damage);
        }
    }
}
