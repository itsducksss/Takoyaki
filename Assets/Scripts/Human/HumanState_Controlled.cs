using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HumanState_Controlled : StateInput
{
    public HumanStateMachine sm;

    public override void OnEnter()
    {
        base.OnEnter();
        sm.IsControlled = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        sm.IsControlled = false;

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        // Move the Human in the direction facing the camera (copied from the player movement)
        Vector3 camForward = sm.PlayerSM.CameraLookDirection;
        Vector3 direction = Vector3.Cross(Vector3.up, camForward);
        Vector3 move = (camForward * sm.MoveDirection.y) + (direction * sm.MoveDirection.x);

        move = move.normalized * sm.MoveSpeed;
        sm.Rb.linearVelocity = new(move.x, sm.Rb.linearVelocity.y, move.z);

        // Move the Humans rb to face the camera
        sm.Rb.MoveRotation(Quaternion.RotateTowards(sm.transform.rotation,
            Camera.main.transform.rotation, sm.CharacterRotateSpeed * Time.fixedDeltaTime));
    }

    #region Inputs
    public override void OnAttatch(InputAction context)
    {
        base.OnAttatch(context);
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
        base.OnJump(context);
        if (context.WasPressedThisFrame()) Jump();
    }

    public override void OnMove(InputAction context)
    {
        base.OnMove(context);
        // MoveDirection provided by the player
        sm.MoveDirection = context.ReadValue<Vector2>();
        Debug.Log($"Move Direction = {sm.MoveDirection}");
    }
    #endregion

    #region Methods

    void Jump()
    {
        if (sm.IsGrounded && sm.CanJump)
        {
            Debug.Log("Jumped");
            sm.Rb.AddForce(Vector3.up * sm.JumpPower, ForceMode.Impulse);
        }
    }

    #endregion
}
