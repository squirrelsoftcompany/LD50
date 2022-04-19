// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-03

using System;
using Attributes;
using Environment;
using Player.ResourcesUsable;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player.Inventory {
public class ItemDrag : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler {
    public ResourceCharacteristics Characteristics { get; set; }

    public event EventHandler<SpawnEventArg> onSpawn;
    public event EventHandler<SpawnEventArg> onBeginDrag;
    [SerializeField] [ReadOnly] private GameObject preSpawn;

    public void OnBeginDrag(PointerEventData eventData) {
        // spawn a visual representation of the area of effect
        var tile = getTileUnderMouse();
        var parent = tile != null ? tile.transform : transform;
        preSpawn = Instantiate(Characteristics.preSpawn, parent);
        preSpawn.transform.localPosition = Vector3.up * 0.01f;
        var resourcePreview = preSpawn.AddComponent<ResourcePreview>();
        resourcePreview.Characteristics = Characteristics;
        preSpawn.SetActive(true);

        onBeginDrag?.Invoke(this, new SpawnEventArg(tile, Characteristics));
    }

    public void OnDrag(PointerEventData eventData) {
        var tile = getTileUnderMouse();
        if (tile != null && preSpawn.transform.parent != tile.transform) {
            preSpawn.transform.SetParent(tile.transform);
            preSpawn.transform.localPosition = Vector3.up * 0.01f;
        }
    }

    private TileGraphic getTileUnderMouse() {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, 1000, ~Characteristics.ignoreLayer)) return null;
        return hit.transform.gameObject.GetComponent<TileGraphic>();
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.localPosition = Vector3.zero;
        // remove visual representation of the AoE
        Destroy(preSpawn);
        var tile = getTileUnderMouse();
        if (tile != null) {
            onSpawn?.Invoke(this, new SpawnEventArg(tile, Characteristics));
        }
    }

    public class SpawnEventArg : EventArgs {
        public ResourceCharacteristics characteristics;
        public TileGraphic tile;

        public SpawnEventArg(TileGraphic tile, ResourceCharacteristics characteristics) {
            this.tile = tile;
            this.characteristics = characteristics;
        }
    }
}
}