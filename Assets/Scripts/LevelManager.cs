using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public static Player[] players;
    public GameObject[] patrolPathsObj;
    public NPCManager npcManager;
    PatrolPath[] patrolPaths;

    // Start is called before the first frame update
    void Start()
    {
        //Gather Patrol Points
        patrolPaths = new PatrolPath[patrolPathsObj.Length];
        for(int i = 0; i < patrolPathsObj.Length; i++)
        {
            patrolPaths[i] = PatrolPath.GeneratePatrolPath(patrolPathsObj[i]);
        }

        foreach (PatrolPath patrolPath in patrolPaths)
            NPCManager.GenerateNPC(patrolPath);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
