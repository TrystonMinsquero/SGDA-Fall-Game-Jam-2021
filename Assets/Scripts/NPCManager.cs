using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCManager : MonoBehaviour
{
    public static NPCManager instance;
    public GameObject NPCPrefab;

    public static List<NPC_Controller> NPC_List;
    public NPC[] NPCTemplates;
    public Weapon[] WeaponTemplates;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
        NPC_List = new List<NPC_Controller>();

        //Added already exisiting NPCs
        NPC_Controller[] alreadyExistingNPCs = GetComponentsInChildren<NPC_Controller>();
        foreach(NPC_Controller npc in alreadyExistingNPCs)
        {
            NPC_List.Add(npc);
        }
    }

    public static void SpawnPlayerOverNPC(PlayerUI playerUI, NPC_Controller npc)
    {
        Debug.Log("Creating new player");
        playerUI.Enable();
        playerUI.transform.position = npc.transform.position;
        playerUI.transform.rotation = npc.transform.rotation;
        playerUI.GetComponent<Player>().TakeOver(npc);
    }

    public static void KillNPC(NPC_Controller npc)
    {
        NPC_List.Remove(npc);
        Destroy(npc.gameObject);
    }

    public static NPC_Controller GetRandomNPC()
    {
        if (NPC_List.Count <= 0)
            return null;
        else
            return NPC_List[Random.Range(0, NPC_List.Count)];
    }

    public static NPC_Controller GenerateNPC(PatrolPath patrolPath, NPC npc = null, Weapon weapon = null)
    {
        NPC_Controller newNPC = Instantiate(instance.NPCPrefab, patrolPath.spawnPoint.position, Quaternion.identity).GetComponent<NPC_Controller>();

        newNPC.AssignComponents();
        newNPC.AssignPatrolPath(patrolPath);

        //Assign NPC
        if (npc != null)
            newNPC.npc = npc;
        else
            newNPC.npc = instance.NPCTemplates[Random.Range(0, instance.NPCTemplates.Length)];

        //Assign Weapon
        if (weapon != null && npc.hasWeapon)
            newNPC.weaponHandler.weapon = weapon;
        else if (newNPC.npc.hasWeapon)
            newNPC.weaponHandler.weapon = instance.WeaponTemplates[Random.Range(0, instance.WeaponTemplates.Length)];
        else
            newNPC.weaponHandler.weapon = null;

        newNPC.AssignComponents();

        NPC_List.Add(newNPC);
        return newNPC;
    }
}
