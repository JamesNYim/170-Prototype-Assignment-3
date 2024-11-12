using UnityEngine;

public class CriminalBehavior : NPCBehavior {
    void Start() {
        SetCriminalStatus(true);
        base.Start();  // Initialize base NPC behavior
          // Set this NPC as a criminal
    }
    public void SetMaterial(Material material) {
        GetComponent<Renderer>().material = material;
    }
}
