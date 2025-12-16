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
        sm.m_PathDestinationNodeIndex = sm.path.UpdatePathDestination(sm.gameObject.transform, sm.m_PathDestinationNodeIndex);

        Vector3 nextDestination = sm.path.GetDestinationPath(sm.gameObject.transform, sm.m_PathDestinationNodeIndex);

        SetNavDestination(nextDestination);

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
}
