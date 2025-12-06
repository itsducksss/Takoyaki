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
    }

    public override void OnMove(InputAction context)
    {
        base.OnMove(context);
    }
    #endregion
}
