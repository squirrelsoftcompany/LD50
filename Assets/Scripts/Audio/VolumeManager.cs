using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager: MonoBehaviour {
    FMOD.Studio.EventInstance instance;
    FMOD.Studio.Bus sfxBus;
    FMOD.Studio.Bus musiqueBus;
    FMOD.Studio.Bus masterBus;


    [SerializeField][Range(-80f, 10f)] 
    private float sfxBusVolume;
    [SerializeField][Range(-80f, 10f)] 
    private float musiqueBusVolume;
    [SerializeField][Range(0.0f, 1f)] 
    private float masterBusVolume = 1.0f;

    private float sfxVolume;
    private float musiqueVolume;

    void Start() {
        sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
        musiqueBus = FMODUnity.RuntimeManager.GetBus("bus:/Musique");
    }

    void Update() {
        sfxVolume = Mathf.Pow(10.0f, (sfxBusVolume - (90f * (1 - masterBusVolume))) / 20f);
        sfxBus.setVolume(sfxVolume);

        musiqueVolume = Mathf.Pow(10.0f, (musiqueBusVolume - (90f * (1 - masterBusVolume))) / 20f);
        musiqueBus.setVolume(musiqueVolume);
    }

    public void Mute()
    {
        masterBusVolume = 0f;
    }

    public void Unmute()
    {
        masterBusVolume = .8f;
    }
}