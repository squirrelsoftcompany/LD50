// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-02

using System;
using System.Collections;
using System.Linq;
using GameEventSystem;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.ResourcesUsable {
[RequireComponent(typeof(Outline), typeof(GameEventListener))]
public abstract class Resource : MonoBehaviour {
    protected ResourceState state = ResourceState.Spawning;
    [SerializeField] private ResourceCharacteristics characteristics;
    [SerializeField] private Material transparent;

    [Range(0.1f, 10f)] [SerializeField] private float outlineMaxWidth = 10f;
    [SerializeField] private Color spawningColor = Color.yellow;
    [SerializeField] private Color cooldownColor = Color.cyan;

    [FormerlySerializedAs("nbFrames")] [SerializeField]
    private int nbFramesPerSecond = 24;

    public ResourceState State => state;

    private float _oneFrameS;

    public int WaitBeforeNextState { get; private set; }

    public ResourceCharacteristics Characteristics => characteristics;

    public event EventHandler<InventoryEventArgs> ReturnToInventory;
    private MeshRenderer _meshRenderer;
    private Material _material;
    private Outline _outline;
    protected abstract void applyEffect();

    private void Awake() {
        #region debug

#if UNITY_EDITOR
        var gameEventListener = GetComponent<GameEventListener>();
        if (gameEventListener.eventAndResponses.Count == 0) {
            throw new ArgumentException(
                "Don't forget to set the ticking event response and call Resource.tick()");
        }

        if (gameEventListener.eventAndResponses.Find(response =>
                response.gameEvent.name == "TickTack") == null) {
            throw new ArgumentException(
                "Don't forget to set the ticking event response and call Resource.tick()");
        }

        if (gameEventListener.eventAndResponses.First(response =>
                response.gameEvent.name == "TickTack").response == null) {
            throw new ArgumentException(
                "Don't forget to set the ticking event response and call Resource.tick()");
        }
#endif

        #endregion

        WaitBeforeNextState = characteristics.spawnWait;
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _material = _meshRenderer.material;
        _meshRenderer.material = transparent;
        _outline = GetComponent<Outline>();
        _outline.OutlineColor = spawningColor;
        _oneFrameS = 1f / nbFramesPerSecond;
    }

    private void Start() {
        StartCoroutine(fadeIn());
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
                if (characteristics.consumable) {
                    state = ResourceState.Finished;
                    WaitBeforeNextState = 0;
                    break;
                }
                state = ResourceState.Cooldown;
                WaitBeforeNextState = Characteristics.cooldown;
                showInCooldown();
                break;
            case ResourceState.Cooldown:
                Debug.Log("Now finished");
                state = ResourceState.Finished;
                if (!characteristics.consumable) {
                    ReturnToInventory?.Invoke(this, new InventoryEventArgs(Characteristics));
                }

                break;
            case ResourceState.Finished:
                Debug.Log("Still finished");
                Destroy(gameObject);
                // just wait to be destroyed, what a journey!
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void showInCooldown() {
        _meshRenderer.material = transparent;
        _outline.enabled = true;
        _outline.OutlineColor = cooldownColor;
        StartCoroutine(fadeOut());
    }

    protected IEnumerator showActive() {
        _meshRenderer.material = _material;
        // _meshRenderer.enabled = true;
        _outline.enabled = false;
        yield return null;
    }


    private IEnumerator fadeIn() {
        _outline.OutlineWidth = 0f;
        var stepIncrease = outlineMaxWidth / (nbFramesPerSecond * characteristics.spawnWait);
        while (_outline.OutlineWidth < outlineMaxWidth) {
            _outline.OutlineWidth += stepIncrease;
            yield return new WaitForSeconds(_oneFrameS);
        }
    }

    protected IEnumerator fadeOut() {
        _outline.OutlineWidth = outlineMaxWidth;
        var currentWidth = _outline.OutlineWidth;

        var stepDecrease = currentWidth / (nbFramesPerSecond * characteristics.cooldown);
        while (_outline.OutlineWidth > 0) {
            _outline.OutlineWidth -= stepDecrease;
            yield return new WaitForSeconds(_oneFrameS);
        }
    }
}
}