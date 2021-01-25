// SID: 1903490
// Date: 11/12/2020

using UnityEngine;

public class RingPickup : Pickup
{
    [SerializeField] protected float multiplierWorth = 0.5f;

    public override void Collected()
    {
        if (GameController.IsMaxMultiplier)
        {
            GameController.Points += worth;
        }
        else
        {
            GameController.Multiplier += multiplierWorth;
        }

        Debug.Log(name + " was collected.");
        Destroy(gameObject, 1.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collected();
        }
    }
}
