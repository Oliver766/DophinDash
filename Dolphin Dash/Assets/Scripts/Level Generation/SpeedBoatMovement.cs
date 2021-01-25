using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoatMovement : MonoBehaviour
{
    public float deactivate = -5;
    private float speed = 50;

    /// <summary>
    /// Moves Speedboat - on the z axis.
    /// </summary>
    void Update()
    {
        if (GameController.IsPlaying == true)
        {
            transform.Translate(0f, 0f, -speed * Time.deltaTime);
        }
        if (transform.position.z < deactivate)
        {
            gameObject.SetActive(false);
        }




    }
}
