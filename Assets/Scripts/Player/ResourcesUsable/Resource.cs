// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-02

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Environment;
using GameEventSystem;
using JetBrains.Annotations;
using Player.Inventory;
using ScriptableObjects;
using UnityEngine;

namespace Player.ResourcesUsable {
[RequireComponent(typeof(Outline), typeof(GameEventListener))]
public abstract class Resource : MonoBehaviour, ITick {
    protected ResourceState state = ResourceState.Spawning;
    [SerializeField] private ResourceCharacteristics characteristics;
    [SerializeField] private Material transparent;

    [Range(0.1f, 10f)] [SerializeField] private float outlineMaxWidth = 10f;
    [SerializeField] private Color spawningColor = Color.yellow;
    [SerializeField] private Color cooldownColor = Color.cyan;

    [SerializeField] protected int fps = 24;
    private TileGraphic _tileGraphic;

    public ResourceState State => state;

    private float _oneFrameS;

    public int WaitBeforeNextState { get; private set; }

    public ResourceCharacteristics Characteristics => characteristics;

    protected HashSet<Vector2Int> neighbours =>
        World.Inst.neighbours(Tile.m_position, Characteristics.rangeOfAction);


    public TileGraphic Tile {
        get => _tileGraphic;
        set => _tileGraphic = value;
    }

    public event EventHandler<InventoryEventArgs> OnLost;

    public event EventHandler<InventoryEventArgs> ReturnToInventory;
    private MeshRenderer _meshRenderer;
    private Material _material;
    private Outline _outline;
    protected abstract void applyEffect();

    protected virtual void Awake() {
        #region debug

#if UNITY_EDITOR
        var gameEventListener = GetComponent<GameEventListener>();
        if (gameEventListener.eventAndResponses.Count == 0) {
            Debug.LogError(
                "Don't forget to set the ticking event response and call Resource.tick()");
        }

        if (gameEventListener.eventAndResponses.Find(response =>
                response.gameEvent.name == "TickTack") == null) {
            Debug.LogError(
                "Don't forget to set the ticking event response and call Resource.tick()");
        }

        if (gameEventListener.eventAndResponses.First(response =>
                response.gameEvent.name == "TickTack").response == null) {
            Debug.LogError(
                "Don't forget to set the ticking event response and call Resource.tick()");
        }
#endif

        #endregion

        WaitBeforeNextState = characteristics.spawnWait;
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _material = _meshRenderer.material;
        _outline = GetComponent<Outline>();
        _outline.OutlineColor = spawningColor;
        _oneFrameS = 1f / fps;
    }

    private void Start() {
        StartCoroutine(spawnAnimation());
    }

    [UsedImplicitly]
    public virtual void tick() {
        // todo call this
        Debug.Log("tick!");
        WaitBeforeNextState--;
        if (State == ResourceState.Active) {
            applyEffect();
        }

        if (WaitBeforeNextState > 0) return;
        switch (State) {
            case ResourceState.Spawning:
                state = ResourceState.Active;
                WaitBeforeNextState = Characteristics.activeDuration;
                StartCoroutine(showActive());
                break;
            case ResourceState.Active:
                if (characteristics.consumable) {
                    state = ResourceState.Finished;
                    WaitBeforeNextState = 0;
                    break;
                }

                state = ResourceState.Cooldown;
                WaitBeforeNextState = Characteristics.cooldown;
                StartCoroutine(showInCooldown());
                break;
            case ResourceState.Cooldown:
                state = ResourceState.Finished;
                if (!characteristics.consumable) {
                    ReturnToInventory?.Invoke(this, new InventoryEventArgs(Characteristics));
                }

                break;
            case ResourceState.Finished:
                Destroy(gameObject);
                // just wait to be destroyed, what a journey!
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected void loseSadly() {
        OnLost?.Invoke(this, new InventoryEventArgs(characteristics));
    }

    protected virtual IEnumerator showInCooldown() {
        _meshRenderer.material = transparent;
        _outline.enabled = true;
        _outline.OutlineColor = cooldownColor;
        _outline.OutlineWidth = outlineMaxWidth;
        var currentWidth = _outline.OutlineWidth;
        var stepDecrease = currentWidth / (fps * characteristics.cooldown);
        while (_outline.OutlineWidth > 0) {
            _outline.OutlineWidth -= stepDecrease;
            yield return new WaitForSeconds(_oneFrameS);
        }
    }

    protected virtual IEnumerator showActive() {
        _meshRenderer.material = _material;
        // _meshRenderer.enabled = true;
        _outline.enabled = false;
        yield return null;
    }


    protected virtual IEnumerator spawnAnimation() {
        _meshRenderer.material = transparent;
        _outline.OutlineWidth = 0f;
        var stepIncrease = outlineMaxWidth / (fps * characteristics.spawnWait);
        while (_outline.OutlineWidth < outlineMaxWidth) {
            _outline.OutlineWidth += stepIncrease;
            yield return new WaitForSeconds(_oneFrameS);
        }
    }
}
}