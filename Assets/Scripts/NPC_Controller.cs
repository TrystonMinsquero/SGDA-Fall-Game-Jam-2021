using UnityEngine;
using System.Collections.Generic;
using Pathfinding;

public class NPC_Controller : MonoBehaviour
{
    public NPC npc;
    public Weapon weapon;
    public float movementSpeed;
    AIPath pathfinder;
    [SerializeField]
    public PatrolPath patrolPath;
    [HideInInspector]
    Patrol patrol;


    // Start is called before the first frame update
    void Start()
    {
        patrol = GetComponent<Patrol>();
        pathfinder = GetComponent<AIPath>();
        pathfinder.maxSpeed = movementSpeed;
        if(patrolPath.patrolpoints.Count > 0)
        {
            AssignPatrolPoints(patrolPath);
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
