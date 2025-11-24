using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwitch : MonoBehaviour
{
    public PlayerController playerController;   //human player movement
    public PlayerController player2Controller; //change to octopus player movement
    public PlayerInput playerInput;
    public PlayerInput playerInput2;
    public Camera cam;  //cam of human
    public Camera cam2; //cam of octopus
    public bool player1Active = true;



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchPlayer();
            PartHit(gameObject);
        }
    }

    public void SwitchPlayer()
    {
        if (player1Active == true)
        {
            playerController.enabled = false;
            player2Controller.enabled = true;
            playerInput.enabled = false;
            playerInput2.enabled = true;
            player1Active = false;
            //cam.enabled = false;
            //cam2.enabled = true;
        }
        else 
        {
            playerController.enabled = true;
            player2Controller.enabled = false;
            playerInput.enabled = true;
            playerInput2.enabled = false;
            player1Active = true;
            //cam.enabled = true;
            //cam2.enabled = false;
            //transform.parent = null; not working like i thought.
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
