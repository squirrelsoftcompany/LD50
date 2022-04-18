using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMananger : MonoBehaviour
{
    public GameEventSystem.GameEvent m_onIntroductionFinishedEvent;
    private Animator m_animator;
    public FMODUnity.StudioEventEmitter m_carEventEmitter;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void onComebackToMenu()
    {
        m_animator.SetTrigger("ToMenu");
    }

    public void onLaunchIntroduction()
    {
        m_animator.SetTrigger("Introduction");
    }

    public void onIntroductionFinished()
    {
        m_onIntroductionFinishedEvent.Raise();
    }

    public void playButtSound()
    {
        m_carEventEmitter.SetParameter("StartMego", 1);
    }

    public void playCarSound()
    {
        m_carEventEmitter.SetParameter("StartCar", 1);
    }
}
