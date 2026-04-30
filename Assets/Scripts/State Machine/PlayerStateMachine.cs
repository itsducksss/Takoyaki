using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public bool CanDetatch { get; set; } = true;
    public bool CanAttatch { get; set; } = true;

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
    public bool IsGrounded { get { return GroundCheck(); } }
    [SerializeField] float _groundCheckDistance = 1f;
    [SerializeField] Vector3 _groundCheckSize = Vector3.one;
    [SerializeField] LayerMask _groundMask;

    [Header("Input")]
    public Vector2 MoveDirection;
    [SerializeField] float _cameraRotateSpeed = 1f;
    public float CameraRotateSpeed { get { return _cameraRotateSpeed; } }
    [Tooltip("The speed of the player characters rotation when following the camera")]
    [SerializeField] float _characterRotateSpeed = 5f;
    public float CharacterRotateSpeed { get { return _characterRotateSpeed; } }

    public GameObject CurrentTarget { get; private set; }
    private List<GameObject> availableTargets = new List<GameObject>();

    [Header("Attatchment")]

    [Tooltip("The multiplier applied to the FreeLook camera when not attatched to a Human")]
    [SerializeField] float _unattatchedCameraRadiusScaleMultiplier = 1f;
    /// <summary> The multiplier applied to the FreeLook camera when not attatched to a Human </summary>
    public float UnattatchedCameraRadiusScaleMultiplier { get { return _unattatchedCameraRadiusScaleMultiplier; } }

    [Tooltip("The multiplier applied to the FreeLook camera when attatched to a Human")]
    [SerializeField] float _attatchedCameraRadiusScaleMultiplier = 1.5f;
    /// <summary> The multiplier applied to the FreeLook camera when attatched to a Human </summary>
    public float AttatchedCameraRadiusScaleMultiplier { get { return _attatchedCameraRadiusScaleMultiplier; } }
    
    private float[] cameraRadiuses;

    [Header("Interaction")]
    public PlayerInventoryManager playerInventoryManager;
    public bool canInteract;
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private float interactDistance = 1f;

    [Header("Components")]

    [SerializeField] Rigidbody _rb;
    public Rigidbody Rb { get { return _rb; } }
    [SerializeField] Collider _characterCollider;
    public Collider CharacterCollider { get { return _characterCollider; } }

    [SerializeField] CinemachineFreeLook _followCamera;
    /// <summary> The Free Look camera following the player (is also used for look input) </summary>
    public CinemachineFreeLook FollowCamera { get { return _followCamera; } }
    public Vector3 CameraLookDirection
    {
        get
        {
            if (_followCamera != null)
            {
                Vector3 forward = Camera.main.transform.forward;
                forward.y = 0f;
                return forward.normalized;
            }
            return Vector3.forward; // In case camera goes missing
        }
    }

    [Header("Sounds")]
    [SerializeField] public AudioSource audioAlerted;
    [SerializeField] public AudioSource audioAttatched, audioDettached, audioJump, audioJibberish, audioLanded;

    public ParticleSystem waterTrail;


    private InputAction moveAction;
    private InputAction attatchAction;
    private InputAction detatchAction;
    private InputAction interactAction;
    private InputAction jumpAction;
    private InputAction lockOnAction;
    #endregion

    private void Awake()
    {
        // Initiate the input actions
        moveAction = InputSystem.actions.FindAction("Move");
        attatchAction = InputSystem.actions.FindAction("Attatch");
        detatchAction = InputSystem.actions.FindAction("Detatch");
        interactAction = InputSystem.actions.FindAction("Interact");
        jumpAction = InputSystem.actions.FindAction("Jump");
        lookAction = InputSystem.actions.FindAction("Look");
        lockOnAction = InputSystem.actions.FindAction("LockOn");

        // Give the player states the reference to this State Machine
        MovementState.sm = this;
        AttatchedState.sm = this;
        waterTrail.Pause();


        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log(moveAction);

        UpdateSensitivity(_cameraRotateSpeed);
    }

    #region Input
    public void OnMove(InputAction context)
    {
        MoveDirection = context.ReadValue<Vector2>();
        waterTrail.Play(); 
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
        // When the player has performed the interact input. Scan an area for interactable objects
        if (context.WasPerformedThisDynamicUpdate())
        {
            var hit = Physics.SphereCastAll(transform.position, interactDistance, Vector3.forward, Mathf.Infinity, interactableLayerMask);

            Debug.Log($"Player Interacted. Count = {hit.Length}");
            if (hit.Length == 0) return;

            if (hit.First().collider.TryGetComponent(out InteractableObject interactable))
                interactable.OnInteract();
        }

        currentState?.OnInteract(context);

    }

    public void OnLockOn(InputAction context)
    {
        currentState?.OnLockOn(context);
    }

    private InputAction lookAction;

    // List of Process Overrides: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Processors.html
    public void UpdateSensitivity(float newSensitivity = 1f) //float xSensitivity, float ySensitivity
    {
        _cameraRotateSpeed = newSensitivity;
        //lookAction.ApplyBindingOverride(new InputBinding { overrideProcessors = $"scale(factor={CameraRotateSpeed})" });
    }
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!FollowCamera)
            Debug.LogError("Missing the FreeLook Camera component");

        SwapState(MovementState);

        CinemachineFreeLook.Orbit[] orbits = _followCamera.m_Orbits;

        cameraRadiuses = new float[]
        {
            orbits[0].m_Radius, orbits[1].m_Radius, orbits[2].m_Radius
        };
    }

    // Update is called once per frame
    void Update()
    {
        OnMove(moveAction);
        OnDetatch(detatchAction);
        OnAttatch(attatchAction);
        OnJump(jumpAction);
        OnLockOn(lockOnAction);
        OnInteract(interactAction);
        //CreateWaterTrail();
            
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
        Debug.Log($"Player Swapped State. Current State: {currentState}");
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

    /// <summary>
    /// Updates the radius of the players VirtualCamera with a given multiplier
    /// </summary>
    public void UpdateCameraRadius(float distanceScaleMultiplier)
    {
        CinemachineFreeLook.Orbit[] orbits = _followCamera.m_Orbits;
        orbits[0].m_Radius = cameraRadiuses[0] * distanceScaleMultiplier;
        orbits[1].m_Radius = cameraRadiuses[1] * distanceScaleMultiplier;
        orbits[2].m_Radius = cameraRadiuses[2] * distanceScaleMultiplier;
    }

    /// <summary>
    /// Cooldown after the player has attatched to a human. Prevents them from mashing on and off the humans head
    /// </summary>
    public IEnumerator AttachmentCooldown()
    {
        CanAttatch = false;
        CanDetatch = false;
        yield return new WaitForSeconds(.1f); // how long cool down is to attach and detach
        CanDetatch = true;
        CanAttatch = true;
    }

    public void OnDetected()
    {
        if (!audioAlerted.isPlaying) audioAlerted.Play();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _attatchDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + (Vector3.down * _groundCheckDistance), _groundCheckSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactDistance);
    }

    private void CreateWaterTrail()
    {
        waterTrail.Play();
    }
}
