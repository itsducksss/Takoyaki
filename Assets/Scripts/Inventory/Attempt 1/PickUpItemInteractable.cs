using UnityEngine;

public class PickUpItemInteractable : InteractableObject
{
    [SerializeField] private Item item;

    public override void OnInteract()
    {
        base.OnInteract();
        player.playerInventoryManager.itemsInInventory.Add(item);
        Destroy(gameObject);
    }
}
