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

        Vector3 camForward = sm.CameraLookDirection;
        Vector3 direction = Vector3.Cross(Vector3.up, camForward);
        Vector3 move = (camForward * sm.MoveDirection.y) + (direction * sm.MoveDirection.x);

        move = move.normalized * sm.MoveSpeed;
        sm.Rb.linearVelocity = new(move.x, sm.Rb.linearVelocity.y, move.z);

        //Vector3 moveTowards = Vector3.RotateTowards(sm.transform.position, move
        //    , Time.deltaTime * sm.CharacterRotateSpeed, Time.deltaTime);
        //sm.Rb.MoveRotation(moveTowards);

        sm.Rb.MoveRotation(Quaternion.RotateTowards(sm.transform.rotation, 
            Camera.main.transform.rotation, sm.CharacterRotateSpeed * Time.fixedDeltaTime));
    }

    #region Inputs
    public override void OnAttatch(InputAction context)
    {
        AttatchToHuman();
    }

    public override void OnDetatch(InputAction context)
    {
        base.OnDetatch(context);
    }

    public override void OnInteract(InputAction context)
    {
        base.OnInteract(context);
    }

    public override void OnJump(InputAction context)
    {
        if(context.WasPressedThisFrame()) Jump();
    }

    public override void OnMove(InputAction context)
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
