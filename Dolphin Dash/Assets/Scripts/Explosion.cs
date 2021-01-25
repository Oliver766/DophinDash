//SID: 1906152
//Date: 11/12/2020

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Animator animator;

    //playing animation on collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("Explosion", true);
            GameController.GameOver();
        }

    }
    //returning animation to initial state when no longer in trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("Explosion", false);

        }
    }

}
