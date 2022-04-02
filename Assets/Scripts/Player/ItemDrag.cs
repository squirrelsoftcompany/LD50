// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-03

using System;
using Environment;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player {
public class ItemDrag : MonoBehaviour, IDragHandler, IEndDragHandler {
    public ResourceCharacteristics Characteristics { get; set; }

    public event EventHandler<SpawnEventArg> onSpawn;

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.localPosition = Vector3.zero;

        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit, 1000)) return;
        // touched something
        var tileGraphic = hit.transform.gameObject.GetComponent<TileGraphic>();
        if (tileGraphic != null) {
            onSpawn?.Invoke(this, new SpawnEventArg(tileGraphic.transform, Characteristics));
        }
    }

    public class SpawnEventArg : EventArgs {
        public ResourceCharacteristics characteristics;
        public Transform transform;

        public SpawnEventArg(Transform transform, ResourceCharacteristics characteristics) {
            this.transform = transform;
            this.characteristics = characteristics;
        }
    }
}
}