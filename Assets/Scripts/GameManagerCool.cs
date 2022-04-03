using System.Collections;
using Attributes;
using GameEventSystem;
using UnityEngine;

public class GameManagerCool : MonoBehaviour {
    [SerializeField] private int bpm;
    [SerializeField] private GameEvent tickTack;
    [SerializeField] private GameEvent showChoiceItem;
    private float waitTime;
    [ReadOnly][SerializeField] private long totalBeats;
    [SerializeField] private long winItemRate = 60;

    public enum GameState {
        eMenuStart = 0,
        eIngame,
        eGameOver,
        eNbState
    }

    public GameState mGameState { get; set; }
    private UIEvents mUIEvents;

    private static GameManagerCool _inst = null;

    public static GameManagerCool Inst {
        get => _inst;
    }

    private void Awake() {
#if UNITY_EDITOR
        if (showChoiceItem == null || tickTack == null) {
            Debug.LogError("The GameManager should have the field completed");
        }
#endif
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
        totalBeats++;
        if (totalBeats % winItemRate == 0L) {
            showChoiceItem.sentBool = true;
            showChoiceItem.Raise();
        }
    }

    public void Play() {
        mGameState = GameState.eIngame;
    }

    public void Menu() {
        mGameState = GameState.eMenuStart;
    }

    public void GameOver() {
        mGameState = GameState.eGameOver;
    }
}