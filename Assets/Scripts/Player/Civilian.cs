using Attributes;
using GameEventSystem;
using UnityEngine;

namespace Player {
public class Civilian : MonoBehaviour, IMortal {
    [SerializeField] private GameEvent deathEvent;
    [SerializeField] private GameEvent gameOver;
    [SerializeField] private GameEvent civilianSaved;
    [SerializeField] private int survivableFire = 2;
    private int _currentFireExposed;
    private int _amountFireExposed;
    [ReadOnly] [SerializeField] private int beingSaved;
    private Animator _animator;
    private static readonly int Death = Animator.StringToHash("Death");
    [SerializeField] private int saveThreshold = 7;
    public bool saved => beingSaved >= saveThreshold;
    private bool dead;
    private static readonly int Save = Animator.StringToHash("Save");
    private static readonly int Saving = Animator.StringToHash("Saving");

    private void Awake() {
        _animator = GetComponentInChildren<Animator>();
    }

    public void doDie() {
        gameOver.Raise();
        Destroy(gameObject);
    }

    public void save() {
        if (saved) return; // don't save twice
        if (dead) return; // well, you know
        beingSaved++;
        if (beingSaved >= saveThreshold) {
            startAnimSaved();
            civilianSaved.sentInt++;
            civilianSaved.Raise();
        } else {
            startAnimSaving();
        }
    }

    private void startAnimSaving() {
        _animator.SetTrigger(Saving);
    }

    private void startAnimSaved() {
        _animator.SetTrigger(Save);
    }

    public void onSaveAnimEnded() {
        Destroy(gameObject);
    }

    public int criticalAmountSurvivable() {
        return survivableFire;
    }

    public void newFireIntensity(int intensity) {
        if (intensity == 0) return;
        _amountFireExposed += intensity;
        if (_amountFireExposed >= criticalAmountSurvivable() && !saved) {
            dead = true;
            _animator.SetTrigger(Death);
        }
    }
}
}