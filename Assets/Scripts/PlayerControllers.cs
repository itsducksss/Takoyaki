using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private Vector2 _moveDirection;
    [SerializeField] float _moveSpeed = 10f;
    public Animator anim;


    [Header("Components")]
    [SerializeField] Rigidbody _hipsrb;

    // Inputs
    InputAction moveAction;
    InputAction jumpAction;
    InputAction throwSelfAction;
    InputAction interactAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        var actionMap = playerInput.currentActionMap;

        moveAction = actionMap.FindAction("Move");
        jumpAction = actionMap.FindAction("Jump");
        throwSelfAction = actionMap.FindAction("Throw Self");
        interactAction = actionMap.FindAction("Interact");

        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;
        jumpAction.performed += OnJump;
        throwSelfAction.performed += OnThrowSelf;
        interactAction.performed += OnInteract;
}
    private void OnEnable()
    {
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        jumpAction.performed += OnJump;
        throwSelfAction.performed += OnThrowSelf;
        interactAction.performed += OnInteract;
    }
    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;

        jumpAction.performed -= OnJump;
        throwSelfAction.performed -= OnThrowSelf;
        interactAction.performed -= OnInteract;

        anim.SetBool("isWalk", false);
        Debug.Log("Me no walkie");

    }

    #region Inputs
    private void OnThrowSelf(InputAction.CallbackContext context)
    {

    }
    private void OnJump(InputAction.CallbackContext context)
    {

    }
    private void OnMove(InputAction.CallbackContext context)
    {
        if(context.performed) Debug.Log($"Player Moving: {_moveDirection = context.ReadValue<Vector2>()}");
        else if(context.canceled) Debug.Log($"Player Stopped Moving: {_moveDirection = Vector2.zero}");
        anim.SetBool("isWalk", true);
        Debug.Log("Walkkkkkkkkkkking");

    }
    private void OnInteract(InputAction.CallbackContext context)
    {

    }
    #endregion

    private void FixedUpdate()
    {
        if(_moveDirection.sqrMagnitude > 0f)
        {
            _hipsrb.linearVelocity = new Vector3(_moveDirection.x, 0, _moveDirection.y) * _moveSpeed;
            Debug.Log($"Moving {_moveDirection}, Magnitude: {_moveDirection.sqrMagnitude}");
        }
    }
}
