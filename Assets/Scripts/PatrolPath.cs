using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct PatrolPath
{
    [SerializeField]
    public Transform spawnPoint;
    [SerializeField]
    public List<Transform> patrolpoints;

    public static PatrolPath GeneratePatrolPath(GameObject obj)
    {
        PatrolPath patrolPath = new PatrolPath();
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        patrolPath.patrolpoints = new List<Transform>();
        foreach (Transform child in children)
        {
            if (child.name == "Spawn Point")
                patrolPath.spawnPoint = child;
            else
                patrolPath.patrolpoints.Add(child);
        }
        return patrolPath;
    }
}

