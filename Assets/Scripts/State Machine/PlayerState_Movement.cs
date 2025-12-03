using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState_Movement : StateInput
{
    public PlayerStateMachine sm;

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

    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

    }

    #region Inputs
    public override void OnAttatch(InputAction.CallbackContext context)
    {
        AttatchToHuman();
    }

    public override void OnDetatch(InputAction.CallbackContext context)
    {
        base.OnDetatch(context);
    }

    public override void OnInteract(InputAction.CallbackContext context)
    {
        base.OnInteract(context);
    }

    public override void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed) Jump();
    }

    public override void OnMove(InputAction.CallbackContext context)
    {
        base.OnMove(context);
    }
    #endregion

    #region Methods

    /// <summary>
    /// Scans in a Sphere Cast and attatches the player to the closest human within a distance
    /// </summary>
    void AttatchToHuman()
    {
        RaycastHit[] hits = Physics.SphereCastAll(sm.transform.position, sm.AttatchDistance, Vector3.forward, Mathf.Infinity, sm.HumanLayerMask);

        if(hits.Length > 0)
        {
            float lowestDist = Mathf.Infinity;
            Vector3 pos = sm.transform.position;
            Collider current = new();

            for (int i = 0; i < hits.Length; i++)
            {
                
                float dist = Vector3.Distance(hits[i].transform.position, pos);

                if (dist < lowestDist)
                {
                    lowestDist = dist;
                    current = hits[i].collider;
                }

            }

            if(current)
            sm.CurrentHuman = current.GetComponent<HumanStateMachine>().ControlledState;
            sm.SwapState(sm.AttatchedState);
        }
        Debug.Log("No Humans Nearby");
    }

    void Jump()
    {
        if (sm.IsGrounded)
        {
            Debug.Log("Jumped");
            sm.Rb.AddForce(Vector3.up * sm.JumpPower, ForceMode.Impulse);
        }
    }

    #endregion
}
