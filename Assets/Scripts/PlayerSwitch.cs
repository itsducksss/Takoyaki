using Unity.Hierarchy;
using UnityEngine;

public class PlayerSwitch : MonoBehaviour
{
    public PlayerController playerController;   //human player movement
    public PlayerController player2Controller; //change to octopus player movement
    public Camera cam;  //cam of human
    public Camera cam2; //cam of octopus
    public bool player1Active = true;



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchPlayer();
        }
    }

    public void SwitchPlayer()
    {
        if (player1Active == true)
        {
            playerController.enabled = false;
            player2Controller.enabled = true;
            player1Active = false;
            cam.enabled = false;
            cam2.enabled = true;
        }
        else 
        {
            playerController.enabled = true;
            player2Controller.enabled = false;
            player1Active = true;
            cam.enabled = true;
            cam2.enabled = false;
        }
    }
}
