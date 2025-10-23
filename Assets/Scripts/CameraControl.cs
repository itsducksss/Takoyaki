using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    //public float rotationSpeed = 1.0f;
    //public Transform root;
    public float deltaRotation;
    [SerializeField] private Rigidbody rb;

    //float mouseX, mouseY;

    //public float stomachOffset;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        deltaRotation += Input.GetAxis("Mouse X");
        //Input.GetAxis("Mouse Y")

        rb.MoveRotation(Quaternion.Euler(0, deltaRotation, 0));
    }
}
