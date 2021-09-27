using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Player[] players;
    public GameObject[] patrolPathsObj;
    PatrolPath[] patrolPaths;

    // Start is called before the first frame update
    void Start()
    {
        patrolPaths = new PatrolPath[patrolPathsObj.Length];
        for(int i = 0; i < patrolPathsObj.Length; i++)
        {

            Transform[] children = patrolPathsObj[i].GetComponentsInChildren<Transform>();
            patrolPaths[i].patrolpoints = new List<Transform>();
            foreach(Transform child in children)
            {
                if (child.name == "Spawn Point")
                    patrolPaths[i].spawnPoint = child;
                else
                    patrolPaths[i].patrolpoints.Add(child);
            }
        }
        foreach(Transform patrolPoint in patrolPaths[0].patrolpoints)
        {
            Debug.Log(patrolPoint.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

struct PatrolPath
{
    public Transform spawnPoint;
    public List<Transform> patrolpoints;
}
