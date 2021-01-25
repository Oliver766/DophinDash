// SID: 1903490
// Date: 11/12/2020

using UnityEngine;

public class ObstacleHitDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.GameOver();
        }
    }
}
