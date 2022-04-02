using System.Collections;
using GameEventSystem;
using UnityEngine;

public class GameManagerCool : MonoBehaviour {
    [SerializeField] private long bpm;
    [SerializeField] private GameEvent tickTack;
    private float waitTime;

    // Start is called before the first frame update
    private void Start() {
        waitTime = (float)bpm / 60;
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
}