using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private FMODUnity.StudioEventEmitter emitter;
    private Environment.World world;

    private int fireLevel = 1;

    // Start is called before the first frame update
    void Start() {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        world = Environment.World.Inst;
    }

    // Called when the fire intensify value change
    public void fireIntensity() {
        float fire = world.m_worldIsOnFire;
        int value = fireLevel;

        if (fireLevel == 1) {
            if (fire > 0.4f) {
                value = 3;
            } else if (fire > 0.3f) {
                value = 2;
            } 
        } else if (fireLevel == 2) {
            if (fire > 0.4f) {
                value = 3;
            } else if (fire < 0.1f) {
                value = 1;
            }
        } else if (fireLevel == 3) {
            if (fire < 0.2f) {
                value = 2;
            } else if(fire < 0.1f) {
                value = 1;
            }
        }

        emitter.SetParameter("Fire", value);
        fireLevel = value;
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
