//SID: 1906152
//Date: 11/12/2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMarkerMovement : MonoBehaviour
{
    //Resets the Level Marker position
    public float locationZ;

    void Update()
    {
        if (GameController.IsPlaying == true)
        {
            transform.Translate(0f, 0f, -GameController.Speed * Time.deltaTime);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.position = new Vector3(0f, 0f,locationZ );
        }
    }
}
