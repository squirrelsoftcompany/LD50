using UnityEngine;

namespace Player.ResourcesUsable {
public class LumberFighter : Resource {
    // Start is called before the first frame update
    protected override void applyEffect() {
        Debug.Log("applying effect " + Characteristics);
        // todo
    }
}
}