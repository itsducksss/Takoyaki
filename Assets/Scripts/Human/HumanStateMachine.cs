using UnityEngine;

public class HumanStateMachine : MonoBehaviour
{
    #region States
    public HumanState_Idle IdleState { get; } = new();
    public HumanState_Patrol PatrolState { get; } = new();
    public HumanState_Controlled ControlledState { get; } = new();

    private State currentState;
    #endregion

    [Header("Attatchment Data")]
    [SerializeField] Transform _headPos;
    public Transform HeadPos { get { return _headPos; } }
    [SerializeField] bool _isControlled;
    public bool IsControlled { get { return _isControlled; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IdleState.sm = this;
        PatrolState.sm = this;
        ControlledState.sm = this;

        SwapState(IdleState);
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.OnUpdate();
    }

    void FixedUpdate()
    {
        currentState?.OnFixedUpdate();
    }

    public void SwapState(State state)
    {
        currentState?.OnExit();
        currentState = state;
        currentState.OnEnter();
    }

    /// <summary>
    /// Method to attatch the player to this humans head
    /// </summary>
    public void PlayerAttatchedToHead(PlayerStateMachine player)
    {
        player.transform.position = _headPos.position; // TODO: Make Cleaner. Tip: Set Rigidbody position to head

    }
}
