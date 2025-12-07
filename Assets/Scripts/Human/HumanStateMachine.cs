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
    public bool IsControlled { get { return _isControlled; } set { _isControlled = value; } }
    public PlayerStateMachine PlayerSM { get; private set; }

    [Header("Stats")]
    [SerializeField] float _jumpPower = 1.0f;
    public float JumpPower { get { return _jumpPower; } }

    [SerializeField] float _moveSpeed = 2.5f;
    public float MoveSpeed { get { return _moveSpeed; } }
    [SerializeField] float _characterRotateSpeed = 5f;
    public float CharacterRotateSpeed { get { return _characterRotateSpeed; } }

    [Header("Ground Check")]
    public bool IsGrounded { get { return GroundCheck(); } }
    [SerializeField] float _groundCheckDistance = 1f;
    [SerializeField] Vector3 _groundCheckSize = Vector3.one;
    [SerializeField] LayerMask _groundMask;

    [Header("Input")]
    public Vector2 MoveDirection { get; set; }
    public bool CanJump = true;

    [Header("Components")]
    [SerializeField] Rigidbody _rb;
    public Rigidbody Rb { get { return _rb; } }

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
        Debug.Log($"Human Swapped State. Current State: {currentState}");
    }

    /// <summary>
    /// Method to attatch the player to this humans head
    /// </summary>
    public void AttatchPlayerToHead(PlayerStateMachine player)
    {
        PlayerSM = player;

        // Set the player as a child to the humans head
        PlayerSM.transform.parent = _headPos;
        PlayerSM.transform.SetLocalPositionAndRotation(Vector3.zero, _headPos.localRotation);


        // Swap to the controlled state
        SwapState(ControlledState);

        Debug.Log($"Player attatched to: {name}");
    }

    public void DetatchPlayerFromHead()
    {
        SwapState(PatrolState);
        Debug.Log($"Player detatched from: {name}");
    }

    /// <summary>
    /// Performs a box cast and returns true if the ground is detected at the players feet
    /// </summary>
    bool GroundCheck()
    {

        bool hit = Physics.BoxCast(transform.position, _groundCheckSize * 0.5f, Vector3.down,
            out RaycastHit rayHit, Quaternion.identity, _groundCheckDistance, _groundMask);

        if (hit)
        {
            Debug.Log("Hit : " + rayHit.collider.name);
            return true;
        }
        else
        {
            Debug.Log("Not Grounded...");
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + (Vector3.down * _groundCheckDistance), _groundCheckSize);
    }
}
