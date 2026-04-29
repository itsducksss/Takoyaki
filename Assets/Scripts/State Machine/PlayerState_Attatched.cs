using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState_Attatched : StateInput
{
    public PlayerStateMachine sm;

    public override void OnEnter()
    {
        base.OnEnter();
        sm.IsAttatched = true;
        sm.Rb.linearVelocity = Vector3.zero;
        sm.Rb.isKinematic = true;
        sm.CharacterCollider.enabled = false;

        sm.UpdateCameraRadius(sm.AttatchedCameraRadiusScaleMultiplier);

        sm.CurrentHuman.sm.AttatchPlayerToHead(sm);

        sm.StartCoroutine(sm.AttachmentCooldown());
    }

    public override void OnExit()
    {
        base.OnExit();
        sm.Rb.isKinematic = false;
        sm.IsAttatched = false;
        sm.CharacterCollider.enabled = true;

        sm.UpdateCameraRadius(sm.UnattatchedCameraRadiusScaleMultiplier);

        // Reset the players rotation
        sm.transform.rotation = Quaternion.identity;

        // Detatch the player from the attatched human
        sm.CurrentHuman.sm.DetatchPlayerFromHead();

        sm.StartCoroutine(sm.AttachmentCooldown());
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
    public override void OnAttatch(InputAction context)
    {
        return;
    }

    public override void OnDetatch(InputAction context)
    {
        if (context.WasPressedThisFrame() && sm.CanDetatch)
        {
            sm.CurrentHuman.OnDetatch(context);

            sm.transform.parent = null;
            sm.SwapState(sm.MovementState);
            sm.audioDettached.Play();
            Debug.Log("Player detatched from the Human");
        }
    }

    public override void OnInteract(InputAction context)
    {
        if (context.WasPressedThisDynamicUpdate())
            sm.CurrentHuman.OnInteract(context);

    }

    public override void OnJump(InputAction context)
    {
        sm.CurrentHuman.OnJump(context);
    }

    public override void OnMove(InputAction context)
    {
        sm.CurrentHuman.OnMove(context);

    }
    #endregion

    #region Methods

    #endregion

}
