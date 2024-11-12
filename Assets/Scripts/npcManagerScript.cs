using UnityEngine;
using System.Collections.Generic;

public class npcManagerScript : MonoBehaviour
{
    public Material civilianMaterial;  
    public Material criminalMaterial; 

    List<GameObject> npcList;
    int NPC_COUNT = 2;
    float criminalSpawnChance = 0.5f;

    void Start()
    {
        npcList = new List<GameObject>();
        GameObject npcPrefab = Resources.Load("NPC") as GameObject;
        GameObject criminalPrefab = Resources.Load("Criminal") as GameObject;
        GameObject civilianPrefab = Resources.Load("Civilian") as GameObject;

        for (int i = 0; i < NPC_COUNT; i++)
        {
            //GameObject npc = Instantiate(npcPrefab);
            //npcList.Add(npc);

            // Randomly assign either CivilianBehavior or CriminalBehavior
            if (Random.value > criminalSpawnChance)
            {
                GameObject civilian = Instantiate(civilianPrefab);
                npcList.Add(civilian);
                //var civilian = npc.AddComponent<CivilianBehavior>();
                //civilian.SetMaterial(civilianMaterial);  // Assign civilian material
            }
            else
            {
                GameObject criminal= Instantiate(criminalPrefab);
                npcList.Add(criminal);
                //var criminal = npc.AddComponent<CriminalBehavior>();
                //criminal.SetMaterial(criminalMaterial);  // Assign criminal material
            }
        }
    }
}


