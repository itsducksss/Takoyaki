using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
                hips.AddForce(1.5f * speed * hips.transform.forward);
            }
            else
            {
                hips.AddForce(hips.transform.forward * speed);
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            hips.AddForce(1.5f * speed * -hips.transform.right);
        }

        if (Input.GetKey(KeyCode.S))
        {
            hips.AddForce(1.5f * speed * -hips.transform.forward);
        }

        if (Input.GetKey(KeyCode.D))
        {
            hips.AddForce(1.5f * speed * hips.transform.right);
        }

        if (Input.GetAxis("Jump") > 0)
        {
            hips.AddForce(new Vector3(0, jumpForce, 0));
            isGrounded = false;
        }
    }

}
