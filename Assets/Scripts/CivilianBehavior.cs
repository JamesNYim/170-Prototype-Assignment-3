using UnityEngine;

public class CivilianBehavior : NPCBehavior {
    void Start() {
        base.Start();  // Initialize base NPC behavior
        SetCriminalStatus(false);  // Set this NPC as a civilian
    }
    public void SetMaterial(Material material) {
        GetComponent<Renderer>().material = material;
    }
}
