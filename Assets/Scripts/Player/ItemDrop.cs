// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-02

using UnityEngine;
using UnityEngine.EventSystems;

namespace Player {
public class ItemDrop : MonoBehaviour, IDropHandler {
    public void OnDrop(PointerEventData eventData) {
        var invPanel = transform as RectTransform;
        if (RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition)) {
            Debug.Log("Dropped");
        }
    }
}
}