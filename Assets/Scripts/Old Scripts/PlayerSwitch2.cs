using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwitch2 : MonoBehaviour
{
    [SerializeField] PlayerController playerController;   //human player movement
    [SerializeField] PlayerController player2Controller; //change to octopus player movement
    [SerializeField] Camera cam;

    public bool player1Active = true;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

    }

    //public void ChangePlayer()
    //{
    //    if (player1Active == true)
    //    {
    //        playerController.enabled = false;
    //        player2Controller.enabled = true;
    //        player1Active = false;
    //        cam.enabled = false;
    //    }
    //    else
    //    {
    //        playerController.enabled = true;
    //        player2Controller.enabled = false;
    //        player1Active = true;
    //        cam.enabled = true;
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PosPlayer"))
        {
            playerController.enabled = false;
            player2Controller.enabled = true;
            player1Active = false;
            Debug.Log("Switch bitch"); // does not work at all this script
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
