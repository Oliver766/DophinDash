//SID: 1906152
//Date: 11/12/2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryPooler : MonoBehaviour
{
    public Transform segmentParent;
    public Dictionary<string, Queue<GameObject>> poolDictionary;


    //holds variables for each pool
    [System.Serializable]
    public class Pool
    {
        public string name;
        public GameObject levelPiece;
        public int size;
    }

    public static SceneryPooler Instance;

    private void Awake()
    {
        Instance = this;
    }


    public List<Pool> pools;


    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        // loops through pool and instantiates new inactive scenery segments on the parent object
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectpool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.levelPiece, segmentParent);
                obj.SetActive(false);
                objectpool.Enqueue(obj);
            }
            poolDictionary.Add(pool.name, objectpool);




        }




    }
    /// <summary>
    /// Takes the Level Segment info and sets them to active with the new position;
    /// </summary>
    /// <param object name="name"></param>
    /// <param new position ="position"></param>
    /// <returns></returns>
    public GameObject SpawnFromPool(string name, Vector3 position)
    {
        if (!poolDictionary.ContainsKey(name))
        {
            Debug.LogWarning("pool with tag " + name + " doesn't exist.");
            return null;
        }


        GameObject objectToSpawn = poolDictionary[name].Dequeue();

        objectToSpawn.SetActive(false);




        objectToSpawn.SetActive(true);


        objectToSpawn.transform.position = position;



        poolDictionary[name].Enqueue(objectToSpawn);




        return null;

    }










   

}
