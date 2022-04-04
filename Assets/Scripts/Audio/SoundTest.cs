using System;
using FMODUnity;
using UnityEngine;

public class SoundTest : MonoBehaviour {
    public int fire = 0;
    private FMODUnity.StudioEventEmitter emitter;

    void Start() {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        emitter.SetParameter("Exit", fire);
    }
    void Update() {
    }
}
