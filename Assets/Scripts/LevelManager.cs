using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        PlayerManager.OnSceneChange(false);

        foreach (PlayerInput playerInput in PlayerManager.players)
        {
            if (playerInput)
            {
                Debug.Log("Spawning Player: " + playerInput);
                NPCManager.SpawnPlayerOverNPC(playerInput.GetComponent<PlayerUI>(), NPCManager.GetRandomNPC());
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
