using UnityEngine;

public class CriminalBehavior : NPCBehavior {
    void Start() {
        base.Start();  // Initialize base NPC behavior
        SetCriminalStatus(true);  // Set this NPC as a criminal
    }
    public void SetMaterial(Material material) {
        GetComponent<Renderer>().material = material;
    }
}
