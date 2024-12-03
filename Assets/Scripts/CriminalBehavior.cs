using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CriminalBehavior : NPCBehavior {
    
    void Start() {
        SetCriminalStatus(true);      // Set this NPC as a criminal
        base.initializeAIBehavior();  // Initialize base NPC behavior
    }

    
    
}
