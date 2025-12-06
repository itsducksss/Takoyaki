using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

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
    public bool IsGrounded { get { return GroundCheck(); } }
    [SerializeField] float _groundCheckDistance = 1f;
    [SerializeField] Vector3 _groundCheckSize = Vector3.one;
    [SerializeField] LayerMask _groundMask;

    [Header("Input")]
    public Vector2 MoveDirection { get; set; }
    [SerializeField] float _cameraRotateSpeed = 1f;
    public float CameraRotateSpeed { get { return _cameraRotateSpeed; } }
    [SerializeField] float _characterRotateSpeed = 5f;
    public float CharacterRotateSpeed { get { return _characterRotateSpeed; } }

    [Header("Components")]
    [SerializeField] Rigidbody _rb;
    public Rigidbody Rb { get { return _rb; } }
    [SerializeField] CinemachineFreeLook _followCamera;
    public CinemachineFreeLook FollowCamera { get { return _followCamera; } }
    public Vector3 CameraLookDirection
    {
        get
        {
            if (_followCamera != null)
            {
                // Get the forward direction of the camera, but ignore the Y component for horizontal movement
                Vector3 forward = Camera.main.transform.forward;
                forward.y = 0f;
                return forward.normalized;
            }
            return Vector3.forward; // Fallback direction
        }
    }


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

        MovementState.sm = this;
        AttatchedState.sm = this;

        Cursor.lockState = CursorLockMode.Locked; // TODO: Temp
    }

    #region Input
    public void OnMove(InputAction context)
    {
        MoveDirection = context.ReadValue<Vector2>();
        currentState?.OnMove(context);
    }

    public void OnAttatch(InputAction context)
    {
        currentState?.OnAttatch(context);
    }

    public void OnJump(InputAction context)
    {
        currentState?.OnJump(context);
    }

    public void OnDetatch(InputAction context)
    {
        currentState?.OnDetatch(context);
    }

    public void OnInteract(InputAction context)
    {
        currentState?.OnInteract(context);
    }
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SwapState(MovementState);

        _followCamera.m_XAxis.m_MaxSpeed = _followCamera.m_XAxis.m_MaxSpeed * _cameraRotateSpeed;
        _followCamera.m_YAxis.m_MaxSpeed = _followCamera.m_YAxis.m_MaxSpeed * _cameraRotateSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        OnMove(moveAction);
        OnAttatch(attatchAction);
        OnDetatch(detatchAction);
        OnInteract(interactAction);
        OnJump(jumpAction);


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
        Debug.Log($"Swapped State. Current State: {currentState}");
    }

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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _attatchDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + (Vector3.down * _groundCheckDistance), _groundCheckSize);
    }
}
