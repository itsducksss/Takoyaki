using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator anim;
    public float speed;
    public float strafeSpeed;
    public float jumpForce;

    public Rigidbody hips;
    public bool isGrounded;

    [SerializeField] Vector3 offset;

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

        if (Input.GetAxis("Jump") > 0)
        {
            hips.AddForce(new Vector3(0, jumpForce, 0));
            isGrounded = false;
        }
        else
        { 
            isGrounded= true;
        }
    }

}
