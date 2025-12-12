using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;
    public float reachDistance = 0.2f;
    public float waitTime = 1f;
    private bool waiting = false;

    private int currentWaypointIndex = 0;

    private int direction = 1;

    private float yPosition;
    public float within_range;
    public Transform targetPlayer;
    [HideInInspector] public bool patrolling = true;
    public NavMeshAgent agent;
    FieldOfView FOV;

    void NextWaypoint()
    {
        if (currentWaypointIndex + direction >= waypoints.Length || currentWaypointIndex + direction < 0)
        {
            direction = -1;
        }

        currentWaypointIndex += direction;
    }

    void Awake()
    {
        yPosition = transform.position.y;
        if (targetPlayer == null)
        {
            if (GameObject.FindWithTag("Player") != null)
            {
                targetPlayer = GameObject.FindWithTag("Player").GetComponent<Transform>();
            }
        }

        FOV = GetComponent<FieldOfView>();
    }

    void Update()
    {
        if (patrolling == false) return;
        if (waypoints.Length == 0 || waiting == true) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        // move to waypoint
        transform.position += direction * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);

        // go back
        if (Vector3.Distance(transform.position, targetWaypoint.position) < reachDistance)
        {
            StartCoroutine(WaitBeforeNext());

            if (PlayerWithinRange()) // FOV.canSeePlayer
            {
                patrolling = false;
                ChasePlayer();
            }
        }
    }

    IEnumerator WaitBeforeNext()
    {
        waiting = true;
        yield return new WaitForSeconds(waitTime);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        waiting = false;
    }

    bool PlayerWithinRange()
    {
        float distance = Vector3.Distance(transform.position, targetPlayer.position);

        if (distance < within_range)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            return true;
        }

        return false;
    }
    private void ChasePlayer()
    {
        agent.SetDestination(targetPlayer.position);
        Debug.Log("stalking");
        agent.speed = 1.5f;
    }
}