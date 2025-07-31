using UnityEngine;

[System.Serializable]
public class ItemIdentifier : MonoBehaviour
{
    [HideInInspector]
    public GameObject originalPrefab;

    public void SetOriginalPrefab(GameObject prefab)
    {
        originalPrefab = prefab;
    }
    
    public GameObject GetOriginalPrefab()
    {
        return originalPrefab;
    }
}
