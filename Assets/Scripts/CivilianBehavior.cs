using UnityEngine;

public class CivilianBehavior : NPCBehavior {
    void Start() {
        SetCriminalStatus(false);
        base.Start();  // Initialize base NPC behavior
          // Set this NPC as a civilian
    }
    public void SetMaterial(Material material) {
        GetComponent<Renderer>().material = material;
    }
}
