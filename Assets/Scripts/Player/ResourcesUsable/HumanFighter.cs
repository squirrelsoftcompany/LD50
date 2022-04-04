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

    public void newFireIntensity(int intensity) {
        if(intensity ==0) return;
        amountFireExposed += intensity;
        if (amountFireExposed >= criticalAmountSurvivable()) {
            loseSadly();
            animator.SetTrigger(Death);
        }
    }

    public abstract void doDie();

}
}