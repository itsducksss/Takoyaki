using UnityEngine;
using UnityEngine.InputSystem;

public class HumanState_Controlled : StateInput
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

    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

    }

    #region Inputs
    public override void OnAttatch(InputAction.CallbackContext context)
    {
        
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
        base.OnJump(context);
    }

    public override void OnMove(InputAction.CallbackContext context)
    {
        base.OnMove(context);
    }
    #endregion
}
