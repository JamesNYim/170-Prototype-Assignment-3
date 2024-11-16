using UnityEngine;

public class CivilianBehavior : NPCBehavior {

    void Start() {
        SetCriminalStatus(false);     // Set this NPC as a civilian
        base.initializeAIBehavior();  // Initialize base NPC behavior
    }


}
