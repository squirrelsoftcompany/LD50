// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-04

using System.Collections;
using Environment;
using GameEventSystem;
using UnityEngine;

namespace Player.ResourcesUsable {
public abstract class HumanFighter : Resource, IMortal {
    protected Animator animator;
    protected FMODUnity.StudioEventEmitter fmod;
    protected int amountFireExposed;
    private bool hasLost;
    private bool _canDie;
    private static readonly int Death = Animator.StringToHash("Death");
    [SerializeField] protected GameEvent deathEvent;
    public abstract int criticalAmountSurvivable();

    protected override void Awake() {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        fmod = GetComponent<FMODUnity.StudioEventEmitter>();
        _canDie = false;
    }

    public override void tick() {
        base.tick();
        if (Tile.m_type == Tile.TileType.eNone) return;
        newFireIntensity(Tile.Intensity);
    }

    protected override IEnumerator showActive() {
        _canDie = true;
        return base.showActive();
    }

    public void newFireIntensity(int intensity) {
        if (hasLost) return;
        if (!_canDie) return;
        if (intensity == 0) return;
        amountFireExposed += intensity;
        if (amountFireExposed >= criticalAmountSurvivable()) {
            loseSadly();
            animator.SetTrigger(Death);
            hasLost = true;
        }
    }

    protected override IEnumerator showInCooldown() {
        fmod.SetParameter("Exit", 1f);
        _canDie = false;
        yield return base.showInCooldown();
    }

    protected void saveCivilians() {
        foreach (var vector2Int in neighboursCivilians) {
            ref var neighbor = ref World.Inst[vector2Int];
            if (!neighbor.HasCivilian) continue;
            var tileGraphic = ChunkGenerator.Inst.get(vector2Int);
            if (tileGraphic.CivilianInstance != null) {
                tileGraphic.CivilianInstance.save();
            }
        }
    }

    public abstract void doDie();
}
}