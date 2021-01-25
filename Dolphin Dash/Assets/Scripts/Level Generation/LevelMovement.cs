//SID: 1906152
//Date: 11/12/2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMovement : MonoBehaviour
{
    


    void Update()
    {
        //Moves the level pieces from - along the z axis
        if (GameController.IsPlaying == true)
        {
            transform.Translate(0f, 0f, -GameController.Speed * Time.deltaTime);
        }
        if (transform.position.z < -33)
        {
            gameObject.SetActive(false);
        }


    }

}
