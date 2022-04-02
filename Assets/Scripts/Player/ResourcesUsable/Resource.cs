// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-02

using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.ResourcesUsable {
public abstract class Resource : MonoBehaviour {
    [SerializeField] protected ResourceState state;
    private bool _askedToChange;
    [SerializeField] protected ResourceCharacteristics characteristics;

    public ResourceState State => state;

    public int WaitBeforeNextState { get; private set; }

    public void spawn() {
        if (state != ResourceState.Idle) return;
        _askedToChange = true;
        state = ResourceState.Spawning;
        WaitBeforeNextState = characteristics.spawnWait;
    }

    
    
    protected abstract void applyEffect();

    public void tick() {
        WaitBeforeNextState--;
        if(WaitBeforeNextState > 0) return;
        switch (State) {
            case ResourceState.Idle:
                break;
            case ResourceState.Spawning:
                state = ResourceState.Active;
                WaitBeforeNextState = characteristics.activeDuration;
                applyEffect();
                break;
            case ResourceState.Active:
                state = ResourceState.Cooldown;
                WaitBeforeNextState = characteristics.cooldown;
                break;
            case ResourceState.Cooldown:
                state = ResourceState.Idle;
                WaitBeforeNextState = 0;
                break;
            case ResourceState.Appearing:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
}