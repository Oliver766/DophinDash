// SID: 1903490
// Date: 11/12/2020

using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Tooltip("The objects to spawn.")]
    [SerializeField] private Transform[] prefabs = null;

    private Transform child;

    private void Awake()
    {
        // Set child if the object already has one
        if (transform.childCount > 0)
        {
            child = transform.GetChild(0);
        }
    }

    private void OnEnable()
    {
        // Called once the level segment is repooled

        if (child || prefabs.Length == 0)
        {
            // Child already exists or no spawnable prefabs exist
            return;
        }

        child = Instantiate(RandomPrefab(), transform);
    }

    private Transform RandomPrefab()
    {
        // Select a random prefab
        int index = UnityEngine.Random.Range(0, prefabs.Length);
        return prefabs[index];
    }
}
