// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-17

using Attributes;
using ScriptableObjects;
using UnityEngine;

namespace Player.ResourcesUsable {
[RequireComponent(typeof(LineRenderer))]
public class ResourcePreview : MonoBehaviour {
    [SerializeField] [ReadOnly] private ResourceCharacteristics characteristics;
    [SerializeField] [ReadOnly] private LineRenderer lineRenderer;
    private float radius;
    private int numSegments = 128;

    public ResourceCharacteristics Characteristics {
        get => characteristics;
        set => characteristics = value;
    }

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        radius = characteristics.rangeOfAction;
    }

    private void Start() {
        doRender();
    }

    private void doRender() {
        lineRenderer.positionCount = numSegments + 1;
        lineRenderer.useWorldSpace = false;
        var deltaTheta = (float)2.0 * Mathf.PI / numSegments;
        var theta = 0f;
        for (var i = 0; i < numSegments + 1; i++) {
            var x = radius * Mathf.Cos(theta);
            var z = radius * Mathf.Sin(theta);
            var pos = new Vector3(x, 0, z);
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }
}
}