using UnityEngine;
using System.Collections.Generic;
using Pathfinding;

public class NPC_Controller : MonoBehaviour
{
    public NPC npc;
    public WeaponHandler weapon;
    public float movementSpeed;
    AIPath pathfinder;
    public GameObject patrolPathObj;
    [HideInInspector]
    public PatrolPath patrolPath;
    [HideInInspector]
    Patrol patrol;


    // Start is called before the first frame update
    void Start()
    {
        patrol = GetComponent<Patrol>();
        pathfinder = GetComponent<AIPath>();
        weapon = GetComponentInChildren<WeaponHandler>();
        pathfinder.maxSpeed = movementSpeed;
        if(patrolPathObj != null)
        {
            AssignPatrolPoints(PatrolPath.GeneratePatrolPath(patrolPathObj));
        }
    }

    public void AssignPatrolPoints(PatrolPath patrolPath)
    {
        patrol.targets = patrolPath.patrolpoints.ToArray();
    }

    private void Update()
    {
    }
}
