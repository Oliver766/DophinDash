// SID: 1903490
// Date: 11/12/2020

using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] protected int worth = 0;

    /// <summary>
    /// Called when the pickup is collected.
    /// </summary>
    public virtual void Collected()
    {
        GameController.Points += worth;
        Debug.Log(name + " was collected.");
        Destroy();
    }

    /// <summary>
    /// Called when the pickup should be destroyed.
    /// </summary>
    protected virtual void Destroy()
    {
        // Destroy the game object
        Destroy(gameObject);
    }
}
