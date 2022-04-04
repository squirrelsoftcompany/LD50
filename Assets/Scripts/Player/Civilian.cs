using GameEventSystem;
using UnityEngine;

namespace Player {
public class Civilian : MonoBehaviour, IMortal {
    [SerializeField] private GameEvent deathEvent;
    [SerializeField] private GameEvent gameOver;
    [SerializeField] private int survivableFire = 2;
    private int _currentFireExposed;
    private int _amountFireExposed;
    private Animator _animator;
    private static readonly int Death = Animator.StringToHash("Death");

    private void Awake() {
        _animator = GetComponentInChildren<Animator>();
    }

    public void doDie() {
        deathEvent.sentString = "Civilian";
        deathEvent.sentBool = true;
        deathEvent.Raise();
        gameOver.Raise();
        Destroy(gameObject);
    }

    public int criticalAmountSurvivable() {
        return survivableFire;
    }

    public void newFireIntensity(int intensity) {
        if (intensity == 0) return;
        _amountFireExposed += intensity;
        if (_amountFireExposed >= criticalAmountSurvivable()) {
            _animator.SetTrigger(Death);
        }
    }
}
}