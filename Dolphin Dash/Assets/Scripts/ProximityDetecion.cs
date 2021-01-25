//SID: 1906152
//Date: 11/12/2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityDetecion : MonoBehaviour

{
    public Animator animator;

    //play animation on trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("Detection", true);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("Detection", false); ;
        }
    }
}
