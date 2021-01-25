using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoatSpawn : MonoBehaviour
{

    public GameObject speedBoat;
    public float waitTime;
    float rnd;
    float temp = 0;





    //float scoreCheck = 150;
    // Start is called before the first frame update
    void Start()
    {






    }
    private void Awake()
    {
        //StartCoroutine(SpawnBoat());


    }
    private void Update()
    {
        if (GameController.IsPlaying == true)
        {
            if (temp < 1)
            {
                StartCoroutine(SpawnBoat());
                temp++;

            }
        }
       
    }


    /// <summary>
    /// Spawns a Boat at either lane every x number of seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnBoat()
    {
        while (true)
        {

            rnd = Random.Range(1, 4);






            if (rnd == 1)
            {
                Debug.Log("spawned");
                GameObject.Instantiate(speedBoat);
                speedBoat.transform.position = new Vector3(-2.7496942f, 0, 75);

            }
            else if (rnd == 2)
            {
                Debug.Log("spawned");
                GameObject.Instantiate(speedBoat);
                speedBoat.transform.position = new Vector3(0, 0, 75);
            }
            else if (rnd == 3)
            {

                Debug.Log("spawned");
                GameObject.Instantiate(speedBoat);
                speedBoat.transform.position = new Vector3(2.749425f, 0, 75);

            }

            yield return new WaitForSeconds(waitTime);

        }
    }



}

