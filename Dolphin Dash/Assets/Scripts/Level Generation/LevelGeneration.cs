//SID: 1906152
//Date: 11/12/2020
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public string[] prefabNames;
    public string[] sceneryPrefabNames;
    string levelName;
    string sceneryName;
    
    public float levelZ;
    public float sceneryZ;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("PlayerDetected");
            //gets a random level/scenery name from the list
            levelName = prefabNames[Random.Range(0, prefabNames.Length)];
            sceneryName = sceneryPrefabNames[Random.Range(0, sceneryPrefabNames.Length)];

            //Sends info to the pooler and runs the spawn function
            LevelPooler.Instance.SpawnFromPool(levelName, transform.position = new Vector3(0, 0, levelZ));
            SceneryPooler.Instance.SpawnFromPool(sceneryName, transform.position = new Vector3(0, 0, sceneryZ));


            //resets the position of the level marker
            transform.position = new Vector3(0, 0, 0);





        }

    }



   
}
