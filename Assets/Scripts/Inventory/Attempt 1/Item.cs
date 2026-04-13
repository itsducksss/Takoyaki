using UnityEngine;

[CreateAssetMenu(menuName = "Items/ Item")]
public class Item : ScriptableObject
{
    [Header("Item Infomation")]
    public string itemName;
    public Sprite itemSprite;
}
