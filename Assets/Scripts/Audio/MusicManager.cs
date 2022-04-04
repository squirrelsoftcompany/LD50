using System;
using FMODUnity;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public float tenseLevel = 0.3f;
    public float hardLevel = 0.5f;

    private FMODUnity.StudioEventEmitter emitter;
    private Environment.World world;

    // Start is called before the first frame update
    void Start() {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        world = Environment.World.Inst;
    }

    // Called when the fire intensify value change
    public void fireIntensity() {
        float fire = world.m_worldIsOnFire;
        int value = 1;

        if(fire > hardLevel) {
            value = 3;
        } else if(fire > tenseLevel) {
            value = 2;
        }

        emitter.SetParameter("Fire", value);
    }

    // Start the music !!!
    public void start() {
        emitter.SetParameter("Fire", 1);
        emitter.SetParameter("StartFire", 1);
        emitter.SetParameter("StartGame", 1);
    }

    // Restart all
    public void goToMenu() {
        emitter.SetParameter("StartFire", 0);
        emitter.SetParameter("Restart", 1);
    }
}
