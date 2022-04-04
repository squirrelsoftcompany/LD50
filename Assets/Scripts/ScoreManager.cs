using System.Collections;
using System.Collections.Generic;
using GameEventSystem;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int m_score = 0;
    public int Score => m_score;
    [SerializeField] private GameEvent civilianSavedEvent;

    public GameEventSystem.GameEvent m_onScoreUpdated = null;

    public int nbCivilianSaved => civilianSavedEvent.sentInt;

    // Start is called before the first frame update
    void Start()
    {
        ResetScore();
    }

    public void IncrementScore()
    {
        m_score++;
        m_onScoreUpdated.sentInt = m_score;
        m_onScoreUpdated?.Raise();
    }

    public void ResetScore()
    {
        m_score = 0;
        m_onScoreUpdated.sentInt = m_score;
        m_onScoreUpdated?.Raise();
    }
}
