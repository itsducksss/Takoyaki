using UnityEngine;
using UnityEngine.InputSystem;

public class Old_PlayerController : MonoBehaviour
{
    public Animator anim;
    public float speed;
    public float strafeSpeed;
    public float jumpForce;

    public Rigidbody hips;
    public bool isGrounded;

    public InputAction playerControls;

    [SerializeField] float groundCheckDistance;

    [SerializeField] Vector3 offset;

    [SerializeField] Transform _groundPoint;
    [SerializeField] LayerMask _groundMask;

    void Start()
    {
        hips = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("isWalk", true);
                anim.SetBool("isRun", true);
                hips.AddForce(1.5f * speed * hips.transform.forward);
            }
            else
            {
                anim.SetBool("isWalk", true);
                anim.SetBool("isRun", false);
                hips.AddForce(hips.transform.forward * speed);
            }
        }
        else
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", false);
        }

            if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("isSideLeft", true);
            hips.AddForce(1.5f * speed * -hips.transform.right);
        }
        else
        {
            anim.SetBool("isSideLeft", false);
        }

        if (Input.GetKey(KeyCode.S))
        {
            anim.SetBool("isWalk", true);
            hips.AddForce(1.5f * speed * -hips.transform.forward);

        }
        else if (!Input.GetKey(KeyCode.W))
        {
            anim.SetBool("isWalk", false);
        }

        if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("isSideRight", true);
            hips.AddForce(1.5f * speed * hips.transform.right);
        }
        else
        {
            anim.SetBool("isSideRight", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            hips.AddForce(new Vector3(0, jumpForce, 0));
            //isGrounded = false;
        }
        else 
        { 
            //isGrounded= true;
        }

        CheckForGround();
    }

    void Update()
    {

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
        Gizmos.DrawWireCube(_groundPoint.position, Vector3.one);
    }
}
