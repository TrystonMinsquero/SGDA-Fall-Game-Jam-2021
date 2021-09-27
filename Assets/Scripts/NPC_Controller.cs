using UnityEngine;
using System.Collections.Generic;
using Pathfinding;

public class NPC_Controller : MonoBehaviour
{
    public NPC npc;
    public WeaponHandler weaponHandler;
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
        AssignComponents();
        if(patrolPathObj != null)
        {
            AssignPatrolPath(PatrolPath.GeneratePatrolPath(patrolPathObj));
        }
    }

    public void AssignComponents()
    {
        patrol = GetComponent<Patrol>();
        pathfinder = GetComponent<AIPath>();
        weaponHandler = GetComponentInChildren<WeaponHandler>();
        pathfinder.maxSpeed = movementSpeed;
    }

    public void AssignPatrolPath(PatrolPath patrolPath)
    {
        this.patrolPath = patrolPath;
        patrol.targets = new Transform[patrolPath.patrolpoints.Length];
        for (int i = 0; i < patrolPath.patrolpoints.Length; i++)
            patrol.targets[i] = patrolPath.patrolpoints[i];
    }

    private void Update()
    {
    }
}
