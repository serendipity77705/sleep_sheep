using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        ItemType item = other.GetComponent<ItemType>();
        if (item != null)
        {
            Debug.Log($"Collected {item.itemType.ToString().ToUpper()} item: {other.name}");
            Destroy(other.gameObject);
        }
    }
}
