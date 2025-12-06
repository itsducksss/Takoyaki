using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState_Attatched : StateInput
{
    public PlayerStateMachine sm;

    public override void OnEnter()
    {
        base.OnEnter();
        sm.IsAttatched = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        sm.IsAttatched = false;

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
        //TODO: Detatch the player from the current human
        sm.CurrentHuman.OnDetatch(context);

    }

    public override void OnInteract(InputAction context)
    {
        sm.CurrentHuman.OnInteract(context);

    }

    public override void OnJump(InputAction context)
    {
        sm.CurrentHuman.OnJump(context);
    }

    public override void OnMove(InputAction context)
    {
        sm.CurrentHuman.OnInteract(context);

    }
    #endregion

}
