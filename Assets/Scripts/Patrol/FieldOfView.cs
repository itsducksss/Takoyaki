using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class FieldOfView : MonoBehaviour
{
    #region Variables
    public float radius;
    public float angle;
    [Range(0,360)]
    public GameObject playerRef;

    public LayerMask targetMask, obstructionMask;

    public bool canSeePlayer;


    //attack but no work
    public float sightRange;
    public float caughtRange;
    public bool playerInSightRange, playerInAttackRange;
    #endregion

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("OctoPlayer");
        StartCoroutine(FOVRoutine());
    }

    #region Field Of View
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }
    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 position = transform.position;
            position.y = target.position.y;
            Vector3 directionToTarget = (target.position - position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (distanceToTarget < caughtRange)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }
    #endregion
}
