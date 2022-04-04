// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-04

using Environment;
using GameEventSystem;
using UnityEngine;

namespace Player.ResourcesUsable {
public abstract class HumanFighter : Resource, IMortal {
    protected Animator animator;
    protected int amountFireExposed;
    private bool hasLost;
    private static readonly int Death = Animator.StringToHash("Death");
    [SerializeField] protected GameEvent deathEvent;
    public abstract int criticalAmountSurvivable();

    protected override void Awake() {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
    }

    public void newFireIntensity(int intensity) {
        if (hasLost) return;
        if (intensity == 0) return;
        amountFireExposed += intensity;
        if (amountFireExposed >= criticalAmountSurvivable()) {
            loseSadly();
            animator.SetTrigger(Death);
            hasLost = true;
        }
    }

    protected void saveCivilians() {
        foreach (var vector2Int in neighboursCivilians) {
            ref var neighbor = ref World.Inst[vector2Int];
            if (!neighbor.HasCivilian) continue;
            var tileGraphic = ChunkGenerator.Inst.get(vector2Int);
            tileGraphic.m_civilian.GetComponent<Civilian>().save();
        }
    }

    public abstract void doDie();
}
}