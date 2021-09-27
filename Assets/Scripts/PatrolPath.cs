using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct PatrolPath
{
    [SerializeField]
    public Transform spawnPoint;
    [SerializeField]
    public Transform[] patrolpoints;

    public static PatrolPath GeneratePatrolPath(GameObject obj)
    {
        PatrolPath patrolPath = new PatrolPath();
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        if (children.Length <= 3)
            Debug.LogError(obj + " must have a patrol path, spawn point, and at least one patrol point");
        patrolPath.patrolpoints = new Transform[children.Length-2];
        int i = 0;
        foreach (Transform child in children)
        {
            if (child.CompareTag("Spawn Point"))
                patrolPath.spawnPoint = child;
            else if (child.CompareTag("Patrol Point"))
            {
                //Debug.Log(" patrolPath.patrolpoints[" + i + "] = " + child);
                patrolPath.patrolpoints[i] = child;
                i++;
            }
        }
        return patrolPath;
    }
}

