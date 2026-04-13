using UnityEngine;

public class DoorInteractable : InteractableObject
{
    [Header("Door")]
    [SerializeField] Animator doorAnimator;

    [Header("Lock Details")]
    [SerializeField] bool isLocked;
    [SerializeField] bool requiresKey;
    [SerializeField] string keyID;

    public override void OnInteract()
    {
        base.OnInteract();
        if (isLocked)
        {
            //option 1
            // if the door requires a key and is currently locked
            if (requiresKey && isLocked)
            {
                foreach (Item item in player.playerInventoryManager.itemsInInventory)
                {
                    //if the key is present, unlock door
                    if (item.itemName == keyID)
                    { 
                        isLocked = false;
                    }
                }
            }
            //if (!isLocked)
            //{
            //    Debug.Log("Door open");
            //    interactableCanvas.SetActive(true);
            //    doorAnimator.Play("Facedoor_temp_pls delete_me");
            //}

        }
    }
}
