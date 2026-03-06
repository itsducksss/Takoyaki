using UnityEngine;
using UnityEngine.AI;

public class HumanState_Patrol : State
{
    public HumanStateMachine sm;

    void Start()
    {
        sm.agent = sm.PlayerSM.GetComponent<NavMeshAgent>();
    }
    public override void OnEnter()
    {
        base.OnEnter();

    }

    public override void OnExit()
    {
        sm.agent.isStopped = true; // makes it so that the nav mesh agent does not continuously make the player move to the set point from the void update function
        sm.agent.ResetPath();
        base.OnExit();

    }

    public override void OnUpdate()
    {
        if (sm.FOV.canSeePlayer == true)
        {
            Debug.Log("I sees u ayyyyyyeeee");

            //sm.transform.LookAt(sm.FOV.playerRef.transform.position);
            float distance = Vector3.Distance(sm.transform.position, sm.FOV.playerRef.transform.position);
            sm.transform.position += sm.transform.forward * 1f * Time.deltaTime;

            sm.agent.isStopped = true; // makes it so that the nav mesh agent does not continuously make the player move to the set point from the void update function
            sm.agent.ResetPath();
        }
        else
        {
            //sm.agent.ResetPath();
            //FunctionTimer.Create(RandomMovementAction, 100f);

            sm.m_PathDestinationNodeIndex = sm.path.UpdatePathDestination(sm.gameObject.transform, sm.m_PathDestinationNodeIndex);

            Vector3 nextDestination = sm.path.GetDestinationPath(sm.gameObject.transform, sm.m_PathDestinationNodeIndex);

            SetNavDestination(nextDestination);
        }

        base.OnUpdate();

    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

    }

    public void SetNavDestination(Vector3 destination)
    {
        if (sm.agent.enabled)
        { 
            sm.agent.SetDestination(destination);
        }
    }

    public void RandomMovementAction()
    {
        if (sm.agent.remainingDistance <= sm.agent.stoppingDistance) // done with the set path
        {
            Vector3 point;
            if (RandomPoint(sm.centrePoint.position, sm.range, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                sm.agent.SetDestination(point);
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; // random point in a sphere
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}
