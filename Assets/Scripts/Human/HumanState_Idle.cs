using UnityEngine;
using UnityEngine.AI;

public class HumanState_Idle : State
{
    public HumanStateMachine sm;

    public override void OnEnter()
    {
        base.OnEnter();

    }

    public override void OnExit()
    {
        base.OnExit();

    }

    public override void OnUpdate()
    {
        base.OnUpdate();
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

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

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
