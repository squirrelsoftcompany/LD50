using GameEventSystem;
using UnityEngine;

namespace Player {
public class Civilian : MonoBehaviour, IMortal, ITick {
    [SerializeField] private GameEvent deathEvent;
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
        deathEvent.Raise();
    }

    public int criticalAmountSurvivable() {
        return survivableFire;
    }

    public void tick() {
        if (_currentFireExposed == 0) return;
        _amountFireExposed += _currentFireExposed;
        if (_amountFireExposed >= criticalAmountSurvivable()) {
            _animator.SetTrigger(Death);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Fire")) {
            // todo get the tile under us in order to get the fire intensity
            // todo put it in amountFireExposed
        }
    }
}
}