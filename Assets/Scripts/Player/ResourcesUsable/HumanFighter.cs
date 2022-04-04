// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-04

using GameEventSystem;
using UnityEngine;

namespace Player.ResourcesUsable {
public abstract class HumanFighter : Resource, IMortal {
    protected Animator animator;
    protected int currentFireExposed;
    protected int amountFireExposed;
    private static readonly int Death = Animator.StringToHash("Death");
    [SerializeField] protected GameEvent deathEvent;
    public abstract int criticalAmountSurvivable();

    protected override void Awake() {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
    }

    public override void tick() {
        base.tick();
        if (currentFireExposed > 0) {
            amountFireExposed += currentFireExposed;
        }

        if (amountFireExposed >= criticalAmountSurvivable()) {
            animator.SetTrigger(Death);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Fire")) {
            // todo get the tile under us in order to get the fire intensity
            // todo put it in amountFireExposed
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Fire")) {
            currentFireExposed = 0;
        }
    }

    public abstract void doDie();
}
}