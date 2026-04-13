using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    protected PlayerStateMachine player;
    [SerializeField] protected GameObject interactableCanvas;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerStateMachine>();
    }

    // Note: Make sure the interactable object is always only detecting the player layer
    private void OnTriggerEnter(Collider other)
    {
        if (player == null)
        {
            player = other.GetComponent<PlayerStateMachine>();
        }
        if (player != null)
        {
            interactableCanvas.SetActive(true);
            player.canInteract = true;
            OnDetected();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (player == null)
        {
            player = other.GetComponent<PlayerStateMachine>();
        }
        if (player != null)
        {
            interactableCanvas.SetActive(false);
            player.canInteract = false;
            OnUndetected();
        }
    }

    /// <summary>
    /// Is called when the player enters the detect trigger zone
    /// </summary>
    protected virtual void OnDetected()
    {

    }

    /// <summary>
    /// Is called when the player leaves the detect trigger zone
    /// </summary>
    protected virtual void OnUndetected()
    {

    }

    /// <summary>
    /// Called when the player has interacted with this object
    /// </summary>
    /// <param name="player"></param>
    public virtual void OnInteract()
    {
        Debug.Log($"Player Interacted with {name}");
    }
}
