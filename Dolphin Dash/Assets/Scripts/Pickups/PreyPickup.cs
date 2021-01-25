// SID: 1903490
// Date: 11/12/2020

using UnityEngine;

public class PreyPickup : Pickup
{
    [Header("References")]
    [SerializeField] private Animator animator = null;
    
    /// <summary>
    /// Called when the prey is initial caught by the dolphin.
    /// </summary>
    public void Caught()
    {
        // Prey is caught, stop any animation that could cause issues
        animator.SetBool("Caught", true);
    }
}
