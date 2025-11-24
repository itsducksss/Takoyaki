using UnityEngine;

public class Water : MonoBehaviour
{
    public Animator anim;

    // Update is called once per frame

    void Start()
    {
        //var rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PartHit(gameObject);
        }
    }

    private void PartHit(GameObject part)
    {
        Destroy(part.GetComponent<ConfigurableJoint>()); //remove appropriate joint to stop the physical joint
        part.transform.parent = null; //clear parent to stop the animation
        //anim.Rebind(); //where anim is the animator. This ensures detached bits stop animating. Clearing parent alone may not work.
        part.GetComponent<Rigidbody>().isKinematic = false; //rb should be set to kinematic when animating. This reverses that meaning it will fall. You may want to do this recursively foreach child of the detaching object.

    }
}
