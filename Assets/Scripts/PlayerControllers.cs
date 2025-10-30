using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private Vector2 _moveDirection;
    [SerializeField] float _moveSpeed = 10f;
    [SerializeField] float _jumpHeight = 10f;
    public Animator anim;
    public bool isGrounded;
    Vector3 velocity;

    [SerializeField] float groundCheckDistance; // changes max height allowed for the character to jump
    [SerializeField] Transform _groundPoint;
    [SerializeField] LayerMask _groundMask;


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

    }

    #region Inputs
    private void OnThrowSelf(InputAction.CallbackContext context)
    {

    }
    private void OnJump(InputAction.CallbackContext context)
    {
        CheckForGround();
        //_hipsrb.AddForce(_moveDirection * 1000f, ForceMode.Impulse);
        //only works when moving

    }
    private void OnMove(InputAction.CallbackContext context)
    {
        if(context.performed) Debug.Log($"Player Moving: {_moveDirection = context.ReadValue<Vector2>()}");
        else if(context.canceled) Debug.Log($"Player Stopped Moving: {_moveDirection = Vector2.zero}");
        anim.SetBool("isWalk", true);
        Debug.Log("Walkkkkkkkkkkking");

        if (context.canceled)
        {
            anim.SetBool("isWalk", false);
            Debug.Log("Me no walkie");
        }

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

    void CheckForGround()
    {
        //Physics.BoxCast(_groundPoint.position, Vector3.one, Vector3.forward, out RaycastHit hit, Quaternion.identity, 2f);
        Physics.BoxCast(_groundPoint.position, Vector3.one * .5f, Vector3.down, out RaycastHit hit, Quaternion.identity, groundCheckDistance, _groundMask);
        Collider collider = hit.collider;

        if (hit.collider)
        {
            //Output the name of the Collider your Box hit
            isGrounded = true;
            Debug.Log("Hit : " + collider.name);

            velocity.y = _jumpHeight;

            _hipsrb.AddForce(velocity * 10000f *Time.deltaTime);
        }
        else
        {
            Debug.Log($"{hit}: {_groundPoint.position}");
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_groundPoint.position, Vector3.one); //for the check for ground()
    }

    //detaching octo from huimannnnnnnn
    public void PartHit(GameObject part)
    {
        Destroy(part.GetComponent<ConfigurableJoint>()); //remove appropriate joint to stop the physical joint
        part.transform.parent = null; //clear parent to stop the animation
        anim.Rebind(); //where anim is the animator. This ensures detached bits stop animating. Clearing parent alone may not work.
        part.GetComponent<Rigidbody>().isKinematic = false; //rb should be set to kinematic when animating. This reverses that meaning it will fall. You may want to do this recursively foreach child of the detaching object.

    }
}
