using UnityEngine;

public class rotateTowardsCamera : MonoBehaviour
{
    private Camera mainCam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCam != null)
        {
            transform.LookAt(transform.position + mainCam.transform.position + Vector3.forward, mainCam.transform.rotation * Vector3.up);
        }
    }
}
