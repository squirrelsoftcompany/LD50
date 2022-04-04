using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMananger : MonoBehaviour
{
    public GameEventSystem.GameEvent m_onIntroductionFinishedEvent;
    private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
