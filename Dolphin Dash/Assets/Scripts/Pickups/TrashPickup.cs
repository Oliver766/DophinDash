// SID: 1903490
// Date: 11/12/2020

using UnityEngine;

public class TrashPickup : Pickup
{
    public override void Collected()
    {
        GameController.ResetMultiplier();
        Debug.Log(name + " was collected.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collected();
        }
    }
}
