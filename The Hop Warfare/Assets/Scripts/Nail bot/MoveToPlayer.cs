using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToPlayer : MonoBehaviour
{
    public GameObject player;
    public NailGuns gun;
    public Difficulty difficulty;

    public float distance;

    public float runAwayDistance;
    public float stopDistance;

    public float[] speed;

    public bool runningAway;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<NavMeshAgent>().speed = speed[difficulty.difficultyIndex];
    }

    // Update is called once per frame
    void Update()
    {
        gun.isRunningAway = runningAway;
        distance = Vector3.Distance(transform.position, player.transform.position);

        Vector3 dirToPlayer = transform.position - player.transform.position;
        Vector3 runAwayPosition = transform.position + dirToPlayer;

        if (distance >= stopDistance)
        {
            gameObject.GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
        }
        else
        {
            gameObject.GetComponent<NavMeshAgent>().SetDestination(transform.position);
            runningAway = false;
        }

        if (distance <= runAwayDistance)
        {
            gameObject.GetComponent<NavMeshAgent>().SetDestination(runAwayPosition);
            runningAway = true;
        }
    }
}
