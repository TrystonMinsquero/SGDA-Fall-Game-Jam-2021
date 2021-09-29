using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public static Player[] players;

    public int _maxPopulation;
    public int _minPopulation;
    public float _spawnDelay;


    private static int maxPopulation;
    private static int minPopulation;
    private static float spawnDelay;

    public GameObject[] patrolPathsObj;

    private static bool needToSpawn;
    private static float canSpawnPlayerTime;
    private static Queue<Player> playersToSpawn;
    private static PatrolPath[] patrolPaths;
    private static Dictionary<PatrolPath, int> patrolPathNPCCount;


    private void Awake()
    {
        if (instance)
            Destroy(this.gameObject);
        else
            instance = this;

        maxPopulation = _maxPopulation;
        minPopulation = _minPopulation;
        spawnDelay = _spawnDelay;
        canSpawnPlayerTime = Time.time;
        needToSpawn = true;

        playersToSpawn = new Queue<Player>();
        patrolPathNPCCount = new Dictionary<PatrolPath, int>();

}

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
            if(playerInput)
                playersToSpawn.Enqueue(playerInput.GetComponent<Player>());
        
    }

    
    //returns all paths in order of number of npc's on path
    private static PatrolPath[] smallestNPCCountPaths()
    {
        PatrolPath[] patrolPathsSorted = new PatrolPath[patrolPathNPCCount.Keys.Count];
        List<KeyValuePair<PatrolPath, int>> myList = patrolPathNPCCount.ToList();

        myList.Sort(
            delegate (KeyValuePair<PatrolPath, int> pair1,
            KeyValuePair<PatrolPath, int> pair2)
            {
                return pair1.Value.CompareTo(pair2.Value);
            }
        );
            int i = 0;
        foreach(KeyValuePair<PatrolPath, int> pair in myList)
            {
                patrolPathsSorted[i] = pair.Key;
            }

        return patrolPathsSorted;
    }
      

    public static bool SpawnNPC()
    {
        foreach (PatrolPath patrolPath in smallestNPCCountPaths())
            if (SafeToSpawn(patrolPath.spawnPoint.transform.position, 4))
            {
                NPCManager.GenerateNPC(patrolPath);
                canSpawnPlayerTime = Time.time + spawnDelay;
                return true;
            }
        return false;
    }
    
    public static bool SpawnPlayer(Player player)
    {
        foreach (NPC_Controller npc in NPCManager.NPC_List)
            if (SafeToSpawn(npc.transform.position, 4))
            {
                NPCManager.SpawnPlayerFromNPC(player.GetComponent<PlayerUI>(), npc);
                return true;
            }
        return false;
    }

    public static bool SafeToSpawn(Vector2 position, float range)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, range);
        foreach (Collider2D collider in colliders)
            if (collider.CompareTag("Player") && collider.GetComponent<Player>().enabled || collider.CompareTag("Projectile"))
                return false;
        return true;

    }
    
    public static void QueuePlayerToSpawn(Player player)
    {
        playersToSpawn.Enqueue(player);
    }

    // Update is called once per frame
    void Update()
    {
        if (playersToSpawn.Count > 0)
        {
            if (SpawnPlayer(playersToSpawn.Peek()))
                playersToSpawn.Dequeue();
        }

        if (NPCManager.NPC_List.Count < minPopulation)
            SpawnNPC();
        else if (NPCManager.NPC_List.Count < maxPopulation && Time.time < canSpawnPlayerTime)
            SpawnNPC();

    }
}
