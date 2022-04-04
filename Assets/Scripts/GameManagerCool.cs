using System.Collections;
using Attributes;
using GameEventSystem;
using Player.Inventory;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class GameManagerCool : MonoBehaviour {
    [SerializeField] public int bpm;
    [SerializeField] private GameEvent tickTack;
    [SerializeField] private GameEvent showChoiceItem;
    [SerializeField] private GameEvent showNotification;
    private float waitTime;
    [ReadOnly] [SerializeField] private long totalBeats;
    [Range(1, 200)] [SerializeField] private long winItemRate = 60;
    private float _formerTimeScale = 1f;
    private Inventory _inventory;

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
        if (showNotification == null || showChoiceItem == null || tickTack == null) {
            Debug.LogError("The GameManager should have the field completed");
        }
#endif
        _inventory = GetComponent<Inventory>();
    }

    // Start is called before the first frame update
    private void Start() {
        if (_inst == null) _inst = this;
        _formerTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        mUIEvents = FindObjectOfType<UIEvents>();
        mGameState = GameState.eMenuStart;
        waitTime = 60f / bpm;
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

        //Trigger notification at the begining to explain the game
        if (totalBeats == 2) //Show after 1 sec
        {
            showNotification.sentString =
                "Oh no the forest is on fire!\n It's up to you to manage the deployment of the fire fighters !";
            showNotification.sentBool = true;
            showNotification.Raise();
        }

        if (totalBeats == 16) //Hide after 8 sec
        {
            showNotification.sentBool = false;
            showNotification.Raise();
        }

        if (totalBeats == 18) {
            // fill the inventory
            _inventory.fillWithStarter();
        }

        if (totalBeats == 20) //Show after 10 sec
        {
            showNotification.sentString =
                "Drag and drop the units and consumable that you have at the bottom of the screen on the map to counter the advance of fire.";
            showNotification.sentBool = true;
            showNotification.Raise();
        }

        if (totalBeats == 34) //Hide after 17 sec)
        {
            showNotification.sentBool = false;
            showNotification.Raise();
        }

        if (totalBeats == winItemRate - 1) //Show at the first shop display
        {
            showNotification.sentString =
                "Here are some reinforcements. Be careful, you can only make one choice each time.";
            showNotification.sentBool = true;
            showNotification.Raise();
        }

        if (totalBeats == winItemRate + 1) //Hide just after shop closed
        {
            showNotification.sentBool = false;
            showNotification.Raise();
        }
    }

    public void Play() {
        mGameState = GameState.eIngame;
        Time.timeScale = _formerTimeScale;
        StartCoroutine(beats());
    }

    public void Menu() {
        mGameState = GameState.eMenuStart;
    }

    public void StartOver() {
        Debug.Log("TODO restart everything!");
        // todo restart fire
        // todo show menu
        // todo restart generation tiles
    }

    public void GameOver() {
        mGameState = GameState.eGameOver;
        _formerTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        StopCoroutine(beats());
    }
}