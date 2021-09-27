using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public List<NPC> NPCs;
    public NPC[] NPCTemplates;

    // Start is called before the first frame update
    void Start()
    {
        NPCs = new List<NPC>();

        //Added already exisiting NPCs
        NPC_Controller[] alreadyExistingNPCs = GetComponentsInChildren<NPC_Controller>();
        foreach(NPC_Controller npc in alreadyExistingNPCs)
        {
            NPCs.Add(npc.npc);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
