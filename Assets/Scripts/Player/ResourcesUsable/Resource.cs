// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-02

using System;
using System.Collections;
using ScriptableObjects;
using UnityEngine;

namespace Player.ResourcesUsable {
public abstract class Resource : MonoBehaviour {
    protected ResourceState state = ResourceState.Spawning;
    private bool _askedToChange = true;
    [SerializeField] private ResourceCharacteristics characteristics;
    

    public ResourceState State => state;

    public int WaitBeforeNextState { get; private set; }

    public ResourceCharacteristics Characteristics => characteristics;

    public event EventHandler<InventoryEventArgs> ReturnToInventory;

    protected abstract void applyEffect();

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
                Debug.Log("Spawning");
                state = ResourceState.Active;
                WaitBeforeNextState = Characteristics.activeDuration;
                break;
            case ResourceState.Active:
                Debug.Log("Active");
                state = ResourceState.Cooldown;
                WaitBeforeNextState = Characteristics.cooldown;
                break;
            case ResourceState.Cooldown:
                Debug.Log("Cooldown");
                state = ResourceState.Finished;
                ReturnToInventory?.Invoke(this, new InventoryEventArgs(Characteristics));
                StartCoroutine(fadeOut());
                break;
            case ResourceState.Finished:
                Debug.Log("Finished");
                // just wait to be destroyed, what a journey!
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IEnumerator fadeOut() {
        var meshRenderer = gameObject.GetComponent<MeshRenderer>();
        var material = meshRenderer.material;
        var materialColor = material.color;
        while (materialColor.a > 0) {
            materialColor.a -= 0.1f;
            material.color = materialColor;
            yield return new WaitForSeconds(0.01f);
        }

        Destroy(this);
    }
}
}