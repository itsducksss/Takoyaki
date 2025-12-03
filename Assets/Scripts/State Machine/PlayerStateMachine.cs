using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]

public class PlayerStateMachine : MonoBehaviour
{
    #region Variables
    private StateInput currentState;
    public PlayerState_Attatched AttatchedState { get; } = new();
    public PlayerState_Movement MovementState { get; } = new();

    [Header("Attatchment")]
    [Tooltip("The distance the player can attatch to a human")]
    [SerializeField] float _attatchDistance = 10f;
    public float AttatchDistance { get { return _attatchDistance; } set { _attatchDistance = value; } }
    [SerializeField] bool _isAttatched;
    public bool IsAttatched { get { return _isAttatched; } set { _isAttatched = value; } }

    [SerializeField] LayerMask _humanLayerMask;
    public LayerMask HumanLayerMask { get { return _humanLayerMask; } set { _humanLayerMask = value; } }
    public HumanState_Controlled CurrentHuman { get { return _controlledHuman; } set { _controlledHuman = value; } }
    [SerializeField] HumanState_Controlled _controlledHuman;

    [Header("Stats")]
    [SerializeField] float _jumpPower = 1.0f;
    public float JumpPower { get { return _jumpPower; } }

    [SerializeField] float _moveSpeed = 2.5f;
    public float MoveSpeed { get { return _moveSpeed; } }
    [Header("Ground Check")]
    [SerializeField] bool _isGrounded;
    public bool IsGrounded { get { _isGrounded = GroundCheck(); return GroundCheck(); } }
    [SerializeField] float _groundCheckDistance = 1f;
    [SerializeField] Vector3 _groundCheckSize = Vector3.one;
    [SerializeField] LayerMask _groundMask;

    [Header("Components")]
    [SerializeField] Rigidbody _rb;
    public Rigidbody Rb { get { return _rb; } }

    private InputAction moveAction;
    private InputAction attatchAction;
    private InputAction detatchAction;
    private InputAction interactAction;
    private InputAction jumpAction;


    #endregion

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        attatchAction = InputSystem.actions.FindAction("Attatch");
        detatchAction = InputSystem.actions.FindAction("Detatch");
        interactAction = InputSystem.actions.FindAction("Interact");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    #region Input
    public void OnMove(InputAction.CallbackContext context)
    {
        currentState?.OnMove(context);
    }

    public void OnAttatch(InputAction.CallbackContext context)
    {
        currentState?.OnAttatch(context);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        currentState?.OnJump(context);
    }

    public void OnDetatch(InputAction.CallbackContext context)
    {
        currentState?.OnDetatch(context);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        currentState?.OnInteract(context);
    }

    void ScanInputs()
    {

    }
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SwapState(MovementState);
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

    public void SwapState(StateInput state)
    {
        currentState?.OnExit();
        currentState = state;
        currentState.OnEnter();
    }

    bool GroundCheck()
    {
        Vector3 pos = transform.position + (Vector3.down * _groundCheckDistance);

        Physics.BoxCast(pos, Vector3.one * .5f, Vector3.down, out RaycastHit hit, Quaternion.identity, _groundCheckDistance, _groundMask);
        Collider collider = hit.collider;

        Debug.Log($"{collider}");

        if (collider)
        {
            Debug.Log("Hit : " + collider.name);
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _attatchDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + (Vector3.down * _groundCheckDistance), _groundCheckSize);
    }
}
