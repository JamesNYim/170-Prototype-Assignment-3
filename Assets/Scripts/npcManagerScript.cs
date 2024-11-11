using UnityEngine;
using System.Collections.Generic;
public class npcManagerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    List<GameObject> npcList;
    int NPC_COUNT = 3;
    void Start()
    {
        npcList = new List<GameObject>();
        GameObject myObject = Resources.Load("NPC") as GameObject;
        for(int i = 0; i < NPC_COUNT; i++){
            npcList.Add(Instantiate(myObject));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
