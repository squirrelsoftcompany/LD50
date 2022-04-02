// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-02

using System;
using System.Collections;
using GameEventSystem;
using ScriptableObjects;
using UnityEngine;

namespace Player.ResourcesUsable {
[RequireComponent(typeof(Outline), typeof(GameEventListener))]
public abstract class Resource : MonoBehaviour {
    protected ResourceState state = ResourceState.Spawning;
    [SerializeField] private ResourceCharacteristics characteristics;
    [SerializeField] private Material transparent;

    public ResourceState State => state;

    public int WaitBeforeNextState { get; private set; }

    public ResourceCharacteristics Characteristics => characteristics;

    public event EventHandler<InventoryEventArgs> ReturnToInventory;
    private MeshRenderer _meshRenderer;
    private Material _material;
    private Outline _outline;
    protected abstract void applyEffect();

    private void Start() {
        WaitBeforeNextState = characteristics.spawnWait;
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _material = _meshRenderer.material;
        _meshRenderer.material = transparent;
        _outline = GetComponent<Outline>();
    }

    public void tick() {
        // todo call this
        Debug.Log("tick!");
        WaitBeforeNextState--;
        if (State == ResourceState.Active) {
            applyEffect();
        }

        if (WaitBeforeNextState > 0) return;
        switch (State) {
            case ResourceState.Spawning:
                Debug.Log("Now active");
                state = ResourceState.Active;
                WaitBeforeNextState = Characteristics.activeDuration;
                StartCoroutine(showActive());
                break;
            case ResourceState.Active:
                Debug.Log("Now in cooldown");
                state = ResourceState.Cooldown;
                WaitBeforeNextState = Characteristics.cooldown;
                StartCoroutine(showInCooldown());
                break;
            case ResourceState.Cooldown:
                Debug.Log("Now finished");
                state = ResourceState.Finished;
                ReturnToInventory?.Invoke(this, new InventoryEventArgs(Characteristics));
                StartCoroutine(fadeOutDestroy());
                break;
            case ResourceState.Finished:
                Debug.Log("Still finished");
                // just wait to be destroyed, what a journey!
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IEnumerator showInCooldown() {
        _meshRenderer.material = transparent;
        _outline.enabled = true;
        _outline.OutlineColor = Color.cyan;
        yield return null;
    }

    protected IEnumerator showActive() {
        _meshRenderer.material = _material;
        // _meshRenderer.enabled = true;
        _outline.enabled = false;
        yield return null;
    }

    protected IEnumerator fadeOutDestroy() {
        var materialColor = _material.color;
        while (materialColor.a > 0) {
            materialColor.a -= 0.1f;
            _material.color = materialColor;
            yield return new WaitForSeconds(0.01f);
        }

        Destroy(gameObject);
    }
}
}