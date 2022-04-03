using System.Collections;
using GameEventSystem;
using UnityEngine;

public class GameManagerCool : MonoBehaviour {
    [SerializeField] private long bpm;
    [SerializeField] private GameEvent tickTack;
    private float waitTime;

    public enum GameState
    {
        eMenuStart = 0,
        eIngame,
        eGameOver,
        eNbState
    }

    public GameState mGameState { get; set; }
    private UIEvents mUIEvents;

    private static GameManagerCool _inst = null;
    public static GameManagerCool Inst
    {
        get => _inst;
    }

    // Start is called before the first frame update
    private void Start() {
        if (_inst == null) _inst = this;
        mUIEvents = FindObjectOfType<UIEvents>();
        mGameState = GameState.eMenuStart;
        waitTime = 60f / bpm;
        StartCoroutine(beats());
    }

    private IEnumerator beats() {
        var currentTime = Time.time;
        while (true) {
            if (Time.time >= currentTime + waitTime) {
                updateBeat();
                currentTime += waitTime;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void updateBeat() {
        tickTack.Raise();
    }

    public void Play()
    {
        mGameState = GameState.eIngame;
    }
}