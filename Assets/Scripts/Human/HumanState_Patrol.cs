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
        sm.agent.isStopped = true;
        sm.agent.ResetPath(); // makes it so that the nav mesh agent does not continously make the player move to the set point from the void udate dunction

        base.OnExit();

    }

    public override void OnUpdate()
    {
        if (sm.agent.remainingDistance <= sm.agent.stoppingDistance) // <---- done with the set path this is the thing that is making the null ref
        {
            Vector3 point;
            if (RandomPoint(sm.centrePoint.position, sm.range, out point)) // does not have a reference fir the centre ppoint transform to do the random movement
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                sm.agent.SetDestination(point);
            }
        }
        base.OnUpdate();

    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result) // this just means that it takes the centre point the picks a random numerical value between 0 and ur set range and uses that to draw a point within the sphere
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
