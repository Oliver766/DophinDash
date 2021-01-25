// SID: 1903490
// Date: 11/12/2020

using System.Collections.Generic;
using UnityEngine;

public class PreyDetection : MonoBehaviour
{
    [SerializeField] private float maxWaitTime = 1f;

    [Header("References")]
    [SerializeField] private Animator animator = null;
    [SerializeField] private Transform[] preyCollectPositions = null;

    private List<PreyPickup> prey = new List<PreyPickup>();
    private float waitTime = 0f;

    private void Update()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;

            if (waitTime <= 0)
            {
                animator.SetBool("Eating", false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PreyPickup pickup = other.GetComponent<PreyPickup>();

        if (pickup)
        {
            // Pickup detected, collect the prey
            Collect(pickup);
        }
    }

    /// <summary>
    /// Collect the prey given in the mouth positions.
    /// </summary>
    /// <param name="pickup">The prey to collect.</param>
    public void Collect(PreyPickup pickup)
    {
        if (!pickup)
        {
            return;
        }

        animator.SetBool("Eating", true);
        pickup.Caught();
        prey.Add(pickup);
        waitTime = maxWaitTime;

        if (prey.Count <= preyCollectPositions.Length)
        {
            pickup.GetComponent<Collider>().enabled = false;

            // Move the prey into position
            pickup.transform.parent = preyCollectPositions[prey.Count - 1];
            pickup.transform.localPosition = Vector3.zero;
            pickup.transform.localEulerAngles = Vector3.zero;
        }
        else
        {
            pickup.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Eats the detected prey.
    /// </summary>
    public void Eat()
    {
        int limit = prey.Count;

        for (int i = 0; i < limit; i++)
        {
            prey[0].Collected();
            prey.RemoveAt(0);
        }
    }
}
